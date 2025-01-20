using Identity.Microservice.Infrastructure.Channels;
using InteractReef.Database.Core;
using InteractReef.Grpc.Base;
using InteractReef.Packets.Schedules;
using InteractReef.Sequrity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Schedules.Microservice.Controllers
{
	[Route("subjects")]
	[ApiController]
	[Authorize]
	public class SubjectController : Controller
	{
		private readonly RoleChannel _roleChannel;

		private readonly IRepository<SubjectItem> _repository;

		private readonly ITokenController _tokenController;

		public SubjectController(
			RoleChannel roleChannel,
			IRepository<SubjectItem> repository,
			ITokenController tokenController)
		{
			_roleChannel = roleChannel;
			_repository = repository;
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

		private async Task<IActionResult> CheckAccess(SubjectItem model)
		{
			var validationResult = ValidateToken(out var userId);
			if (validationResult != null) return validationResult;

			var request = new IdListRequest();
			request.Params.Add(userId);
			request.Params.Add(model.OrgId);

			var isEmployee = await _roleChannel.RoleService.IsEmployeeAsync(request);

			if (!isEmployee.Result) return Unauthorized("No access");

			return null;
		}

		[HttpGet]
		public IActionResult Get([FromQuery] int id)
		{
			var item = _repository.GetById(id);
			return item != null ? Ok(item) : NotFound();
		}

		[HttpPost("add")]
		public async Task<IActionResult> Add([FromBody] SubjectItem model)
		{
			var errors = await CheckAccess(model);
			if (errors != null) return errors;

			_repository.Add(model);
			return Ok();
		}

		[HttpPost("update")]
		public async Task<IActionResult> Update([FromBody] SubjectItem model)
		{
			var errors = await CheckAccess(model);
			if (errors != null) return errors;

			_repository.Update(model.Id, model);
			return Ok();
		}

		[HttpPost("delete")]
		public async Task<IActionResult> Delete([FromQuery] int id)
		{
			var schedule = _repository.GetById(id);
			if (schedule == null)
				return NotFound();

			var errors = await CheckAccess(schedule);
			if (errors != null) return errors;

			_repository.Delete(schedule);
			return Ok();
		}
	}
}
