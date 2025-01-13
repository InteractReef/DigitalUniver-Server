using Identity.Microservice.Infrastructure.Channels;
using InteractReef.Database.Core;
using InteractReef.Packets;
using InteractReef.Sequrity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Organizations.Microservice.Controllers
{
	public class EmployeesController : Controller
	{
		private readonly OrganizationChannel _organizationChannel;
		private readonly IRepository<EmployeeModel> _employeesRepository;

		private readonly ITokenController _tokenController;

		public EmployeesController(
			OrganizationChannel organizationChannel,
			IRepository<EmployeeModel> employeesRepository,
			ITokenController tokenController)
		{
			_organizationChannel = organizationChannel;
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

		private IActionResult CheckAccess(EmployeeModel employee)
		{
			var validationResult = ValidateToken(out int userId);
			if (validationResult != null) return validationResult;

			var invoker = _employeesRepository.GetById(userId);

			if (invoker == null || invoker.OrganizationId != employee.OrganizationId) return Unauthorized("No access");

			if (invoker.Level < 1 || invoker.Level <= employee.Level) return BadRequest("No access");

			if (employee.Level > 2 || employee.Level < 0) return BadRequest("Invalid level");

			return null;
		}

		[HttpGet("{id}")]
		public IActionResult GetById([FromQuery] int id)
		{
			var item = _employeesRepository.GetById(id);
			if(item == null) return NotFound();

			return Ok(item);
		}

		[HttpGet("org/{id}")]
		public IActionResult GetByOrganization([FromQuery] int id)
		{
			var items = _employeesRepository.GetAll().Where(x => x.OrganizationId == id);

			return Ok(items);
		}

		[HttpPost("add")]
		public async Task<IActionResult> Add([FromBody] EmployeeModel employee)
		{
			var error = CheckAccess(employee);
			if (error != null) return error;

			var reqeust = new InteractReef.Grpc.Organizations.GetById()
			{
				Id = employee.OrganizationId,
			};

			var exist = await _organizationChannel.OrganizationService.OrganizationExistsAsync(reqeust);
			if (!exist.Result)
			{
				return NotFound();
			}

			_employeesRepository.Add(employee);
			return Ok();
		}

		[HttpPost("update")]
		public async Task<IActionResult> Update([FromBody] EmployeeModel employee)
		{
			var error = CheckAccess(employee);
			if (error != null) return error;

			var reqeust = new InteractReef.Grpc.Organizations.GetById()
			{
				Id = employee.OrganizationId,
			};

			var exist = await _organizationChannel.OrganizationService.OrganizationExistsAsync(reqeust);
			if (!exist.Result)
			{
				return NotFound();
			}

			_employeesRepository.Update(employee.Id, employee);
			return Ok();
		}

		[HttpPost("delete")]
		public IActionResult Delete([FromQuery] int id) 
		{
			var item = _employeesRepository.GetById(id);
			if(item == null) return NotFound();

			var error = CheckAccess(item);
			if (error != null) return error;

			_employeesRepository.Delete(item);
			return Ok();
		} 
	}
}
