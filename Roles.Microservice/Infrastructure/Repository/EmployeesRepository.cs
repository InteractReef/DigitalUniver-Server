using InteractReef.Database.Core;
using InteractReef.Packets;
using Microsoft.EntityFrameworkCore;
using Roles.Microservice.Infrastructure.Database;

namespace Roles.Microservice.Infrastructure.Repository
{
	public class EmployeesRepository : GenericRepository<RolesDbContext, EmployeeModel>
	{
		public EmployeesRepository(RolesDbContext dbContext)
		: base(dbContext)
		{
		}

		protected override DbSet<EmployeeModel> DbSet => _dbContext.Employees;
	}
}
