using Grpc.Net.Client;
using InteractReef.Grpc.Organizations;

namespace Identity.Microservice.Infrastructure.Channels
{
	public class OrganizationChannel
	{
		private readonly GrpcChannel _channel;

		public OrganizationGrpcService.OrganizationGrpcServiceClient OrganizationService { get; private set; }

		public OrganizationChannel()
		{
			_channel = GrpcChannel.ForAddress("http://organizations-service:5007");
			OrganizationService = new OrganizationGrpcService.OrganizationGrpcServiceClient(_channel);
		}
	}
}
