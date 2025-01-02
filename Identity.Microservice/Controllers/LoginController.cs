using InteractReef.Database.Core;
using InteractReef.Packets.Identity;
using Microsoft.AspNetCore.Mvc;
using Identity.Microservice.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using InteractReef.Sequrity;

namespace Identity.Microservice.Controllers
{
	[ApiController]
	[Route("identity/login")]
	public class LoginController : Controller
	{
		private readonly IRepository<UserModel> _repository;
		private readonly ITokenController _tokenController;

		protected LoginController(IRepository<UserModel> repository, ITokenController tokenController)
		{
			_repository = repository;
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			var result = await _repository.GetAll().SingleOrDefaultAsync(
				x => x.Email == request.email 
				&& x.Password == request.password);

			if(result == null)
			{
				return NotFound();
			}

			var token = _tokenController.CreateToken(result.Id, result.Email, result.Password);

			return Ok(token);
		}
	}
}
