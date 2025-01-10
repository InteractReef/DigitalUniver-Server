using InteractReef.Database.Core;
using InteractReef.Packets.User;
using Microsoft.AspNetCore.Authorization;
using InteractReef.Sequrity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Users.Microservice.Controllers
{
	[Route("users")]
	[ApiController]
	[Authorize]
	public class UserController : Controller
	{
		private readonly ITokenController _tokenController;
		private readonly IRepository<UserModel> _repository;

		public UserController(IRepository<UserModel> repository, ITokenController tokenController) 
		{
			_repository = repository;
			_tokenController = tokenController;
		}


		[HttpPost("get/{id}")]
		public IActionResult GetById(int id)
		{
			var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

			if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
			{
				return Unauthorized("JWT token is missing or invalid.");
			}

			var token = authHeader.Substring("Bearer ".Length).Trim();

			var values = _tokenController.GetValues(token, new List<string>() { ClaimTypes.NameIdentifier });
			if (values == null || values.Count == 0)
				return Unauthorized("JWT token is missing or invalid.");

			if (!int.TryParse(values[ClaimTypes.NameIdentifier], out var result))
			{
				return Unauthorized();
			}

			if (id != result)
			{
				return BadRequest();
			}

			var userInfo = _repository.GetById(id);
			return userInfo != null ? Ok(userInfo) : NotFound();
		}

		[HttpGet("list")]
		public IActionResult GetList(int startId, int amount)
		{
			if (startId <= 1 || amount <= 1) return BadRequest();
			if (amount > 40) return BadRequest("Too much amount");

			var users = _repository.GetAll().Take(amount).Where(x => x.Id >= startId);
			return Ok(users);
		}

		[HttpPost("delete")]
		public IActionResult DeleteUser(int id)
		{
			var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

			if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
			{
				return Unauthorized("JWT token is missing or invalid.");
			}

			var token = authHeader.Substring("Bearer ".Length).Trim();

			var values = _tokenController.GetValues(token, new List<string>() { ClaimTypes.NameIdentifier });
			if (values == null || values.Count == 0)
				return Unauthorized("JWT token is missing or invalid.");

			if (!int.TryParse(values[ClaimTypes.NameIdentifier], out var result)){
				return Unauthorized();
			}

			if(id != result)
			{
				return BadRequest();
			}

			var user = _repository.GetById(id);
			if (user == null)
				return NotFound();

			_repository.Delete(user);
			return Ok();
		}

		[HttpPost("update")]
		public IActionResult UpdateUser([FromBody] UserModel userModel)
		{
			var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

			if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
			{
				return Unauthorized("JWT token is missing or invalid.");
			}

			var token = authHeader.Substring("Bearer ".Length).Trim();

			var values = _tokenController.GetValues(token, new List<string>() { ClaimTypes.NameIdentifier });
			if (values == null || values.Count == 0)
				return Unauthorized("JWT token is missing or invalid.");

			if (!int.TryParse(values[ClaimTypes.NameIdentifier], out var result))
			{
				return Unauthorized();
			}

			var id = userModel.Id;

			if (id != result)
			{
				return BadRequest();
			}

			var user = _repository.GetById(id);
			if (user == null)
				return NotFound();

			_repository.Update(id, userModel);
			return Ok();
		}
	}
}
