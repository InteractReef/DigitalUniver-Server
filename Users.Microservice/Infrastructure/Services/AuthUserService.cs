using Grpc.Core;
using InteractReef.Database.Core;
using InteractReef.Grpc.Base;
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

		public override async Task<GrpcResponse> GetUser(GetUserRequest request, ServerCallContext context)
		{
			var user = await _repository.GetAll().SingleOrDefaultAsync(x => x.Email == request.Email && x.Password == request.Password);
			if (user == null)
				return new GrpcResponse() { Status = GrpcStatus.NotFound };

			return new GrpcResponse() { Status = GrpcStatus.Ok, IntResponse = new IntResponse() { Result = user.Id } };
		}

		public override async Task<GrpcResponse> TryAddUser(UserInfoModel request, ServerCallContext context)
		{
			var emailUsed = await _repository.GetAll().FirstOrDefaultAsync(x => x.Email == request.Email);
			if (emailUsed != null)
				return new GrpcResponse() { Status = GrpcStatus.AlreadyExist };

			var userModel = new UserModel()
			{
				Email = request.Email,
				Password = request.Password,
			};
			_repository.Add(userModel);
			return new GrpcResponse() { Status = GrpcStatus.Ok };
		}
	}
}
