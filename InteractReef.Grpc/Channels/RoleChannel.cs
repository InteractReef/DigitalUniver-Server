using Grpc.Net.Client;
using InteractReef.Grpc.Roles;
using InteractReef.Grpc.Users;

namespace Identity.Microservice.Infrastructure.Channels
{
	public class RoleChannel
	{
		private readonly GrpcChannel _channel;

		public RoleGrpcService.RoleGrpcServiceClient RoleService { get; private set; }

		public RoleChannel()
		{
			_channel = GrpcChannel.ForAddress("http://roles-service:5009");
			RoleService = new RoleGrpcService.RoleGrpcServiceClient(_channel);
		}
	}
}
