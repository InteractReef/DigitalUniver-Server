using InteractReef.Database.Core;
using InteractReef.Packets.Organizations;
using InteractReef.Sequrity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Organizations.Microservice.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class StudentsController : ControllerBase
	{
		private readonly IRepository<StudentModel> _studentsRepository;
		private readonly IRepository<OrganizationModel> _organizationsRepository;

		private readonly ITokenController _tokenController;

		public StudentsController(
			IRepository<StudentModel> students, 
			IRepository<OrganizationModel> organization,
			ITokenController tokenController)
		{
			_organizationsRepository = organization;
			_studentsRepository = students;
			_tokenController = tokenController;
		}

		private IActionResult ValidateToken(int targetId, out int userId)
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

			if (userId != targetId)
				return Unauthorized("Invalid login data");

			return null;
		}

		[HttpGet("group/{id}")]
		public IActionResult GetByGroup([FromQuery] int id)
		{
			var items = _studentsRepository.GetAll().Where(x => x.GroupId == id);
			return Ok(items);
		}

		[HttpGet("{id}")]
		public IActionResult GetById([FromQuery] int id)
		{
			var item = _studentsRepository.GetById(id);
			if (item == null) return NotFound();
			return Ok(item);
		}

		[HttpPost("add")]
		public IActionResult Add([FromBody] StudentModel student) 
		{
			var error = ValidateToken(student.UserId, out var userId);
			if (error != null) return error;

			var org = _organizationsRepository.GetById(student.OrganizationId);
			if (org == null) return NotFound("Organization not found");
			if (org.Groups.FirstOrDefault(x => x.Id == student.GroupId) == null) return NotFound("Group not found");

			_studentsRepository.Add(student);
			return Ok();
		}

		[HttpPost("update")]
		public IActionResult Update([FromBody] StudentModel student) 
		{
			var error = ValidateToken(student.UserId, out var userId);
			if (error != null) return error;

			_studentsRepository.Update(student.Id, student);
			return Ok();
		}

		[HttpPost("delete")]
		public IActionResult Delete([FromQuery] int id)
		{
			var error = ValidateToken(id, out var userId);
			if (error != null) return error;

			var item = _studentsRepository.GetById(id);
			if (item == null) return NotFound();

			_studentsRepository.Delete(item);
			return Ok();
		}
	}
}
