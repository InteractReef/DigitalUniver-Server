using InteractReef.Database.Core;
using InteractReef.Packets.Organizations;
using Microsoft.EntityFrameworkCore;
using Organizations.Microservice.Infrastructure.Database;

namespace Organizations.Microservice.Infrastructure.Repository
{
	public class StudentsRepository : GenericRepository<OrganizationsDbContext, StudentModel>
	{
		public StudentsRepository(OrganizationsDbContext dbContext) : base(dbContext)
		{
		}

		protected override DbSet<StudentModel> DbSet => _dbContext.Students;
	}
}
