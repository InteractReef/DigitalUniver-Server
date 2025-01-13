using Grpc.Core;
using InteractReef.Database.Core;
using InteractReef.Grpc.Organizations;
using InteractReef.Packets.Organizations;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Organizations.Microservice.Infrastructure.GrpcService
{
	public class OrganizationService : OrganizationGrpcService.OrganizationGrpcServiceBase
	{
		private readonly IRepository<OrganizationModel> _repository;

		public OrganizationService(IRepository<OrganizationModel> repository) 
		{
			_repository	= repository;
		}

		public override async Task<ExistResponce> OrganizationExists(GetById request, ServerCallContext context)
		{
			var org = _repository.GetById(request.Id);
			return await Task.FromResult(new ExistResponce() { Result = org != null });
		}

		public override async Task<ExistResponce> GroupExists(GetGroupById request, ServerCallContext context)
		{
			var exists = await _repository.GetAll().AnyAsync(org => org.Id == request.Organizationid && org.Groups.Any(group => group.Id == request.GroupId));
			return new ExistResponce() { Result = exists };
		}
	}
}
