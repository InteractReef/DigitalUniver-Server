using Identity.Microservice.Infrastructure.Channels;
using InteractReef.Grpc.Users;
using InteractReef.Packets.Identity;
using Microsoft.AspNetCore.Mvc;
using InteractReef.Grpc.Base;

namespace Identity.Microservice.Controllers
{
	[ApiController]
	[Route("identity/auth")]
	public class RegController : Controller
	{
		private UserChannel _userChannel;

		public RegController(UserChannel userChannel)
		{
			_userChannel = userChannel;
		}

		[HttpPost("reg")]
		public async Task<IActionResult> Register([FromBody] RegisterRequest request)
		{
			var userInfo = new UserInfoModel()
			{
				Email = request.email,
				Password = request.password,
			};

			var responce = await _userChannel.UserService.TryAddUserAsync(userInfo);
			if(responce.Status != GrpcStatus.Ok)
			{
				return BadRequest(responce.Status);
			}

			return Ok();
		}
	}
}
