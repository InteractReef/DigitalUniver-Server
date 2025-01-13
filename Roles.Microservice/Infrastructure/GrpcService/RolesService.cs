using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InteractReef.Database.Core;
using InteractReef.Grpc.Roles;
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

		public override async Task<IsGroupMemberResponce> IsAdmin(IsAdminRequest request, ServerCallContext context)
		{
			var user = _adminRepository.GetById(request.UserId);
			return await Task.FromResult(new IsGroupMemberResponce() { Result = user != null });
		}

		public override async Task<IsGroupMemberResponce> IsEmployee(IsEmployeeRequest request, ServerCallContext context)
		{
			var user = await _employeeRepository.GetAll().FirstOrDefaultAsync(
				x => x.OrganizationId == request.OrganizationId 
				&& x.UserId == request.UserId);

			return new IsGroupMemberResponce() { Result = user != null };
		}

		public override async Task<IsGroupMemberResponce> IsStudent(IsStudentRequest request, ServerCallContext context)
		{
			var user = await _studentRepository.GetAll().FirstOrDefaultAsync(
				x => x.OrganizationId == request.OrganizationId
				&& x.GroupId == request.GroupId
				&& x.UserId == request.UserId);

			return new IsGroupMemberResponce() { Result = user != null };
		}

		public override async Task<Empty> SetAdmin(SetAdminRequest request, ServerCallContext context)
		{
			var model = new AdminModel()
			{
				UserId = request.UserId,
			};
			_adminRepository.Add(model);
			return await Task.FromResult(new Empty());
		}

		public override async Task<Empty> SetEmployee(SetEmployeeRequest request, ServerCallContext context)
		{
			var model = new EmployeeModel()
			{
				UserId = request.UserId,
				OrganizationId = request.OrganizationId,
			};
			_employeeRepository.Add(model);
			return await Task.FromResult(new Empty());
		}

		public override async Task<Empty> SetStudent(SetStudentRequest request, ServerCallContext context)
		{
			var model = new StudentModel()
			{
				UserId = request.UserId,
				OrganizationId = request.OrganizationId,
				GroupId = request.GroupId,
			};
			_studentRepository.Add(model);
			return await Task.FromResult(new Empty());
		}
	}
}
