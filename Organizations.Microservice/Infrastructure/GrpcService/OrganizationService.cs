using Grpc.Core;
using InteractReef.Database.Core;
using InteractReef.Grpc.Base;
using InteractReef.Grpc.Organizations;
using InteractReef.Packets.Organizations;
using Microsoft.AspNetCore.Http.HttpResults;
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

		public override async Task<BoolResponse> OrganizationExists(IdRequest request, ServerCallContext context)
		{
			var org = _repository.GetById(request.Id);
			return await Task.FromResult(new BoolResponse() { Result = org != null });
		}

		public override async Task<GrpcResponse> GroupExists(IdListRequest request, ServerCallContext context)
		{
			if (request.Params.Count < 2) return new GrpcResponse() { Status = GrpcStatus.BadRequest };

			var exists = await _repository.GetAll().AnyAsync(
				org => org.Id == request.Params[0] 
				&& org.Groups.Any(group => group.Id == request.Params[1]));

			return new GrpcResponse() 
			{ 
				Status = exists ? GrpcStatus.Ok : GrpcStatus.NotFound,
				BoolResponse = new BoolResponse() { Result = exists } 
			};
		}
	}
}
