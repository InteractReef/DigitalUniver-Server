using Identity.Microservice.Infrastructure.Channels;
using InteractReef.Database.Core;
using InteractReef.Packets;
using InteractReef.Packets.Organizations;
using InteractReef.Sequrity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Organizations.Microservice.Controllers
{
	[ApiController]
	[Authorize]
	[Route("org")]
	public class OrganizationsController : ControllerBase
	{
		private readonly RoleChannel _roleChannel;

		private readonly IRepository<OrganizationModel> _orgRepository;
		private readonly IRepository<EmployeeModel> _employeesRepository;
		
		private readonly ITokenController _tokenController;

		public OrganizationsController(
			RoleChannel roleChannel,
			IRepository<OrganizationModel> orgRepository, 
			IRepository<EmployeeModel> employeesRepository,
			ITokenController tokenController) 
		{
			_roleChannel = roleChannel;
			_orgRepository = orgRepository;
			_employeesRepository = employeesRepository;
			_tokenController = tokenController;
		}

		private IActionResult ValidateToken(out int userId)
		{
			userId = 0;

			var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
			if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
			{
				return Unauthorized("JWT token is missing or invalid.");
			}

			var token = authHeader.Substring("Bearer ".Length).Trim();
			var values = _tokenController.GetValues(token, new List<string> { ClaimTypes.NameIdentifier });

			if (values == null || values.Count == 0 || !int.TryParse(values[ClaimTypes.NameIdentifier], out userId))
			{
				return Unauthorized("JWT token is missing or invalid.");
			}

			return null;
		}

		private async Task<IActionResult> CheckAccess(OrganizationModel model)
		{
			var validationResult = ValidateToken(out var userId);
			if (validationResult != null) return validationResult;

			var isAdmin = await _roleChannel.RoleService.IsAdminAsync(new InteractReef.Grpc.Roles.IsAdminRequest() { UserId = userId });
			if (isAdmin.Result) return null;

			var employee = _employeesRepository.GetAll().FirstOrDefault(x => x.UserId == userId);
			if (employee == null || employee.Level < 2 || employee.OrganizationId != model.Id) return Unauthorized("No access");

			return null;
		}

		[HttpGet("range")]
		public IActionResult GetRange([FromQuery] int startId, [FromQuery, Range(0, 20)] int count)
		{
			if (startId < 0) return BadRequest("Invalid value");

			var items = _orgRepository.GetAll().Take(count).Where(x => x.Id >= startId);

			return Ok(items);
		}

		[HttpGet("all")]
		public IActionResult GetAll() 
		{
			var items = _orgRepository.GetAll();
			return Ok(items);
		}

		[HttpGet("{id}")]
		public IActionResult GetById([FromQuery] int id)
		{
			var item = _orgRepository.GetById(id);
			return item != null ? Ok(item) : NotFound();
		}

		[HttpPost("add")]
		public async Task<IActionResult> Add([FromBody] OrganizationModel model)
		{
			var error = await CheckAccess(model);
			if (error != null) return error;

			_orgRepository.Add(model);
			return Ok();
		}

		[HttpPost("update")]
		public async Task<IActionResult> Update([FromBody] OrganizationModel model)
		{
			var error = await CheckAccess(model);
			if (error != null) return error;

			_orgRepository.Update(model.Id, model);
			return Ok();
		}

		[HttpPost("delete")]
		public async Task<IActionResult> Delete([FromQuery] int id) 
		{
			var item = _orgRepository.GetById(id);
			if (item == null) return NotFound();

			var error = await CheckAccess(item);
			if (error != null) return error;

			_orgRepository.Delete(item);
			return Ok(item);
		}
	}
}
