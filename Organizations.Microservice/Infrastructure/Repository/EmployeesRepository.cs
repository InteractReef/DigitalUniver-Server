using InteractReef.Database.Core;
using InteractReef.Packets.Organizations;
using Microsoft.EntityFrameworkCore;
using Organizations.Microservice.Infrastructure.Database;

namespace Organizations.Microservice.Infrastructure.Repository
{
	public class EmployeesRepository : GenericRepository<OrganizationsDbContext, EmployeeModel>
	{
		public EmployeesRepository(OrganizationsDbContext dbContext) 
			: base(dbContext)
		{
		}

		protected override DbSet<EmployeeModel> DbSet => _dbContext.Employees;
	}
}
