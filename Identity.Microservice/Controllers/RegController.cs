using Identity.Microservice.Infrastructure.Database;
using InteractReef.Database.Core;
using InteractReef.Packets.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Microservice.Controllers
{
	[ApiController]
	[Route("identity/reg")]
	public class RegController : Controller
	{
		private readonly IRepository<UserModel> _repository;

		protected RegController(IRepository<UserModel> repository)
		{
			_repository = repository;
		}

		[HttpPost]
		public async Task<IActionResult> Register([FromBody] RegRequest request)
		{
			var emailIsUsed = await _repository.GetAll().AnyAsync(x => x.Email == request.email);
			if (emailIsUsed)
			{
				return BadRequest(new RegisterRequest(IdentityStatusCode.EmailAlreadyUsed));
			}

			var user = new UserModel()
			{
				Email = request.email,
				Password = request.password,
			};

			_repository.Add(user);

			return Ok();
		}
	}
}
