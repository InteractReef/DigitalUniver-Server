using Grpc.Core;
using InteractReef.Database.Core;
using InteractReef.Grpc.Roles;
using InteractReef.Grpc.Base;
using InteractReef.Packets;
using Microsoft.EntityFrameworkCore;

namespace Roles.Microservice.Infrastructure.GrpcService
{
	public class RolesService : RoleGrpcService.RoleGrpcServiceBase
	{
		private readonly IRepository<AdminModel> _adminRepository;
		private readonly IRepository<EmployeeModel> _employeeRepository;
		private readonly IRepository<StudentModel> _studentRepository;

		public RolesService(
			IRepository<AdminModel> adminRepository,
			IRepository<EmployeeModel> employeeRepository,
			IRepository<StudentModel> studentRepository) 
		{
			_adminRepository = adminRepository;
			_employeeRepository = employeeRepository;
			_studentRepository = studentRepository;
		}

		public override async Task<BoolResponce> IsAdmin(IdRequest request, ServerCallContext context)
		{
			var user = _adminRepository.GetById(request.Id);
			return await Task.FromResult(new BoolResponce() { Result = user != null });
		}

		public override async Task<BoolResponce> IsEmployee(MultiplyIdRequest request, ServerCallContext context)
		{
			if(request.Params.Count < 2) return new BoolResponce() { Result = false };

			var user = await _employeeRepository.GetAll().FirstOrDefaultAsync(
				x => x.OrganizationId == request.Params[0] 
				&& x.UserId == request.Params[1]);

			return new BoolResponce() { Result = user != null };
		}

		public override async Task<BoolResponce> IsStudent(MultiplyIdRequest request, ServerCallContext context)
		{
			if (request.Params.Count < 3) return new BoolResponce() { Result = false };

			var user = await _studentRepository.GetAll().FirstOrDefaultAsync(
				x => x.UserId == request.Params[0]
				&& x.GroupId == request.Params[1]
				&& x.OrganizationId == request.Params[2]);

			return new BoolResponce() { Result = user != null };
		}

		public override async Task<GrpcResponce> SetAdmin(IdRequest request, ServerCallContext context)
		{
			var model = new AdminModel()
			{
				UserId = request.Id,
			};
			_adminRepository.Add(model);
			return await Task.FromResult(new GrpcResponce() { Status = GrpcStatus.Ok });
		}

		public override async Task<GrpcResponce> SetEmployee(MultiplyIdRequest request, ServerCallContext context)
		{
			if (request.Params.Count < 2) return await Task.FromResult(new GrpcResponce() { Status = GrpcStatus.BadRequest });

			var model = new EmployeeModel()
			{
				UserId = request.Params[0],
				OrganizationId = request.Params[1],
			};
			_employeeRepository.Add(model);

			return await Task.FromResult(new GrpcResponce() { Status = GrpcStatus.Ok });
		}

		public override async Task<GrpcResponce> SetStudent(MultiplyIdRequest request, ServerCallContext context)
		{
			if (request.Params.Count < 3) return await Task.FromResult(new GrpcResponce() { Status = GrpcStatus.BadRequest });

			var model = new StudentModel()
			{
				UserId = request.Params[0],
				GroupId = request.Params[1],
				OrganizationId = request.Params[2],
			};
			_studentRepository.Add(model);
			return await Task.FromResult(new GrpcResponce() { Status = GrpcStatus.Ok });
		}
	}
}
