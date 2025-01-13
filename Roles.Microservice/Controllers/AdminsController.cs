using InteractReef.Database.Core;
using InteractReef.Packets;
using InteractReef.Sequrity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Organizations.Microservice.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class AdminsController : ControllerBase
	{
		private readonly IRepository<AdminModel> _adminsRepository;

		private readonly ITokenController _tokenController;

		public AdminsController(
			IRepository<AdminModel> admins,
			ITokenController tokenController)
		{
			_adminsRepository = admins;
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

		[HttpGet("{id}")]
		public IActionResult GetById([FromQuery] int id)
		{
			var item = _adminsRepository.GetById(id);
			if (item == null) return NotFound();
			return Ok(item);
		}

		[HttpPost("add")]
		public IActionResult Add([FromBody] AdminModel admin) 
		{
			var error = ValidateToken(admin.UserId, out var userId);
			if (error != null) return error;

			_adminsRepository.Add(admin);
			return Ok();
		}

		[HttpPost("update")]
		public IActionResult Update([FromBody] AdminModel admin) 
		{
			var error = ValidateToken(admin.UserId, out var userId);
			if (error != null) return error;

			_adminsRepository.Update(admin.Id, admin);
			return Ok();
		}

		[HttpPost("delete")]
		public IActionResult Delete([FromQuery] int id)
		{
			var error = ValidateToken(id, out var userId);
			if (error != null) return error;

			var item = _adminsRepository.GetById(id);
			if (item == null) return NotFound();

			_adminsRepository.Delete(item);
			return Ok();
		}
	}
}
