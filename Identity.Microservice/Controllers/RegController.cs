using Grpc.Core;
using Identity.Microservice.Infrastructure.Channels;
using InteractReef.Grpc.Users;
using InteractReef.Packets.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Microservice.Controllers
{
	[ApiController]
	[Route("identity/auth")]
	public class RegController : Controller
	{
		private UserChannel _channel;

		public RegController(UserChannel channel)
		{
			_channel = channel;
		}

		[HttpPost("reg")]
		public async Task<IActionResult> Register([FromBody] RegisterRequest request)
		{
			var userInfo = new UserInfoModel()
			{
				Email = request.email,
				Password = request.password,
			};

			try
			{
				await _channel.UserService.TryAddUserAsync(new AddUserRequest() { UserInfo = userInfo });
			}
			catch(RpcException e)
			{
				return BadRequest(e.Message);
			}

			return Ok();
		}
	}
}
