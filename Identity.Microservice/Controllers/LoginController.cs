using InteractReef.Packets.Identity;
using Microsoft.AspNetCore.Mvc;
using InteractReef.Sequrity;
using Identity.Microservice.Infrastructure.Channels;
using InteractReef.Grpc.Users;
using InteractReef.Grpc.Base;
using System.Security.Claims;

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
			try
			{
				var responce =  await _channel.UserService.GetUserAsync(userRequest);

				if(responce.Status != GrpcStatus.Ok)
				{
					return NotFound();
				}

				var claims = new List<Claim>()
				{
					new Claim(ClaimTypes.NameIdentifier, responce.IntResponce.Result.ToString()),
					new Claim(ClaimTypes.Email, request.email),
					new Claim(ClaimTypes.UserData, request.password)
				};
				var token = _tokenController.CreateToken(claims);
				return Ok(token);
			}
			catch (Exception ex)
			{
				return NotFound();
			}
		}
	}
}
