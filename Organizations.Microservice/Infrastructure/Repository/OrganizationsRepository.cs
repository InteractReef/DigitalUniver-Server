using InteractReef.Database.Core;
using InteractReef.Packets.Organizations;
using Microsoft.EntityFrameworkCore;
using Organizations.Microservice.Infrastructure.Database;

namespace Organizations.Microservice.Infrastructure.Repository
{
	public class OrganizationsRepository : GenericRepository<OrganizationsDbContext, OrganizationModel>
	{
		public OrganizationsRepository(OrganizationsDbContext dbContext) 
			: base(dbContext)
		{
		}

		protected override DbSet<OrganizationModel> DbSet => _dbContext.Organizations;
	}
}
