using Identity.Microservice.Infrastructure.Channels;
using InteractReef.Database.Core;
using InteractReef.Packets;
using InteractReef.Grpc.Base;
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
		private readonly OrganizationChannel _organizationChannel;
		private readonly IRepository<StudentModel> _studentsRepository;

		private readonly ITokenController _tokenController;

		public StudentsController(
			OrganizationChannel organizationChannel,
			IRepository<StudentModel> students,
			ITokenController tokenController)
		{
			_organizationChannel = organizationChannel;
			_studentsRepository = students;
			_tokenController = tokenController;
		}

		private IActionResult ValidateToken(int targetId, out int userId)
		{
			userId = 0;

			var token = _tokenController.GetToken(HttpContext);
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("JWT token is missing or invalid.");
			}

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
		public async Task<IActionResult> Add([FromBody] StudentModel student) 
		{
			var error = ValidateToken(student.UserId, out var userId);
			if (error != null) return error;

			var reqeust = new MultiplyIdRequest();
			reqeust.Params.Add(student.OrganizationId);
			reqeust.Params.Add(student.GroupId);

			var exist = await _organizationChannel.OrganizationService.GroupExistsAsync(reqeust);
			if (exist.Status != GrpcStatus.Ok)
			{
				switch (exist.Status)
				{
					case GrpcStatus.NotFound: return NotFound();
					case GrpcStatus.BadRequest: return BadRequest();
				}
			}

			_studentsRepository.Add(student);
			return Ok();
		}

		[HttpPost("update")]
		public async Task<IActionResult> Update([FromBody] StudentModel student) 
		{
			var error = ValidateToken(student.UserId, out var userId);
			if (error != null) return error;

			var reqeust = new MultiplyIdRequest();
			reqeust.Params.Add(student.OrganizationId);
			reqeust.Params.Add(student.GroupId);

			var exist = await _organizationChannel.OrganizationService.GroupExistsAsync(reqeust);
			if (exist.Status != GrpcStatus.Ok)
			{
				switch (exist.Status)
				{
					case GrpcStatus.NotFound: return NotFound();
					case GrpcStatus.BadRequest: return BadRequest();
				}
			}

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
