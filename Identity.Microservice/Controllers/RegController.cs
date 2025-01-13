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
		private UserChannel _userChannel;
		private RoleChannel _roleChannel;

		public RegController(UserChannel userChannel, RoleChannel roleChannel)
		{
			_userChannel = userChannel;
			_roleChannel = roleChannel;
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
				await _userChannel.UserService.TryAddUserAsync(userInfo);
			}
			catch(RpcException e)
			{
				return BadRequest(e.Message);
			}

			return Ok();
		}
	}
}
