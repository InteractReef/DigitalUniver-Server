using InteractReef.Packets.Identity;
using Microsoft.AspNetCore.Mvc;
using InteractReef.Sequrity;
using Identity.Microservice.Infrastructure.Channels;
using InteractReef.Grpc.Users;

namespace Identity.Microservice.Controllers
{
	[ApiController]
	[Route("identity/auth")]
	public class LoginController : Controller
	{
		private UserChannel _channel;
		private readonly ITokenController _tokenController;

		public LoginController(UserChannel channel, ITokenController tokenController)
		{
			_channel = channel;
			_tokenController = tokenController;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			var userRequest = new GetUserRequest() { Email = request.email, Password = request.password };

			var token = "";
			try
			{
				var userInfo =  await _channel.UserService.GetUserAsync(userRequest);

				if (userInfo == null)
				{
					return NotFound();
				}

				token = _tokenController.CreateToken(userInfo.Id, userInfo.Email, userInfo.Password);
			}
			catch (Exception ex)
			{
				return NotFound();
			}

			return Ok(token);
		}
	}
}
