using InteractReef.Database.Core;
using InteractReef.Packets.Organizations;
using InteractReef.Sequrity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Organizations.Microservice.Controllers
{
	public class EmployeesController : Controller
	{
		private readonly IRepository<OrganizationModel> _orgRepository;
		private readonly IRepository<EmployeeModel> _employeesRepository;

		private readonly ITokenController _tokenController;

		public EmployeesController(
			IRepository<OrganizationModel> orgRepository,
			IRepository<EmployeeModel> employeesRepository,
			ITokenController tokenController)
		{
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
		public IActionResult Add([FromBody] EmployeeModel employee)
		{
			var error = CheckAccess(employee);
			if (error != null) return error;

			_employeesRepository.Add(employee);
			return Ok();
		}

		[HttpPost("update")]
		public IActionResult Update([FromBody] EmployeeModel employee)
		{
			var error = CheckAccess(employee);
			if (error != null) return error;

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
