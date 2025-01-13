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

		public override async Task<BoolResponce> OrganizationExists(IdRequest request, ServerCallContext context)
		{
			var org = _repository.GetById(request.Id);
			return await Task.FromResult(new BoolResponce() { Result = org != null });
		}

		public override async Task<GrpcResponce> GroupExists(MultiplyIdRequest request, ServerCallContext context)
		{
			if (request.Params.Count < 2) return new GrpcResponce() { Status = GrpcStatus.BadRequest };

			var exists = await _repository.GetAll().AnyAsync(
				org => org.Id == request.Params[0] 
				&& org.Groups.Any(group => group.Id == request.Params[1]));

			return new GrpcResponce() 
			{ 
				Status = exists ? GrpcStatus.Ok : GrpcStatus.NotFound,
				BoolResponce = new BoolResponce() { Result = exists } 
			};
		}
	}
}
