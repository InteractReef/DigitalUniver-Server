using InteractReef.Database.Core;
using Microsoft.AspNetCore.Authorization;
using InteractReef.Sequrity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InteractReef.Packets.Users;

namespace Users.Microservice.Controllers
{
	[Route("users")]
	[ApiController]
	[Authorize]
	public class UsersController : ControllerBase
	{
		private readonly ITokenController _tokenController;
		private readonly IRepository<UserModel> _repository;

		public UsersController(IRepository<UserModel> repository, ITokenController tokenController) 
		{
			_repository = repository;
			_tokenController = tokenController;
		}

		private IActionResult ValidateToken(int invokerId, out int userId)
		{
			userId = -1;

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

			if(userId != invokerId)
				return BadRequest("Access denied.");

			return null;
		}

		[HttpPost("get/{id}")]
		public IActionResult GetById(int id)
		{
			var validationResult = ValidateToken(id, out var userId);
			if (validationResult != null) return validationResult;

			var userInfo = _repository.GetById(id);
			return userInfo != null ? Ok(userInfo) : NotFound();
		}

		[HttpPost("delete")]
		public IActionResult DeleteUser(int id)
		{
			var validationResult = ValidateToken(id, out var userId);
			if (validationResult != null) return validationResult;

			var user = _repository.GetById(id);
			if (user == null) return NotFound();

			_repository.Delete(user);
			return Ok();
		}

		[HttpPost("update")]
		public IActionResult UpdateUser([FromBody] UserModel userModel)
		{
			var validationResult = ValidateToken(userModel.Id, out var userId);
			if (validationResult != null) return validationResult;

			var user = _repository.GetById(userModel.Id);
			if (user == null) return NotFound();

			_repository.Update(userModel.Id, userModel);
			return Ok();
		}
	}
}
