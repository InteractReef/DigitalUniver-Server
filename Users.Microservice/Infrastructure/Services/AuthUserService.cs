using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InteractReef.Database.Core;
using InteractReef.Grpc.Users;
using InteractReef.Packets.Users;
using Microsoft.EntityFrameworkCore;

namespace Users.Microservice.Infrastructure.Services
{
	public class AuthUserService : AuthUsersService.AuthUsersServiceBase
	{
		private readonly IRepository<UserModel> _repository;

		public AuthUserService(IRepository<UserModel> repository) 
		{
			_repository = repository;
		}

		public override async Task<GetUserResponce> GetUser(GetUserRequest request, ServerCallContext context)
		{
			var user = await _repository.GetAll().SingleOrDefaultAsync(x => x.Email == request.Email && x.Password == request.Password);
			if (user == null)
				throw new RpcException(new Status(StatusCode.NotFound, string.Empty));

			var userInfo = new UserInfoModel()
			{
				Id = user.Id,
				Email = request.Email,
				Password = request.Password,
			};

			return new GetUserResponce() { InfoModel = userInfo };
		}

		public override async Task<Empty> TryAddUser(AddUserRequest request, ServerCallContext context)
		{
			var emailUsed = await _repository.GetAll().FirstOrDefaultAsync(x => x.Email == request.UserInfo.Email);
			if (emailUsed != null)
				throw new RpcException(new Status(StatusCode.AlreadyExists, string.Empty));

			var userModel = new UserModel()
			{
				Email = request.UserInfo.Email,
				Password = request.UserInfo.Password,
			};
			_repository.Add(userModel);
			return new Empty();
		}
	}
}
