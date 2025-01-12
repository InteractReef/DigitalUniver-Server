using Grpc.Net.Client;
using InteractReef.Grpc.Users;

namespace Identity.Microservice.Infrastructure.Channels
{
	public class UserChannel
	{
		private readonly GrpcChannel _channel;

		public AuthUsersService.AuthUsersServiceClient UserService { get; private set; }

		public UserChannel()
		{
			_channel = GrpcChannel.ForAddress("http://users-service:5003");
			UserService = new AuthUsersService.AuthUsersServiceClient(_channel);
		}
	}
}
