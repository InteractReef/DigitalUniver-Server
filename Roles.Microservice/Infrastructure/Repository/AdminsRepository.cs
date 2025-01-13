using InteractReef.Database.Core;
using InteractReef.Packets;
using Microsoft.EntityFrameworkCore;
using Roles.Microservice.Infrastructure.Database;

namespace Roles.Microservice.Infrastructure.Repository
{
	public class AdminsRepository : GenericRepository<RolesDbContext, AdminModel>
	{
		public AdminsRepository(RolesDbContext dbContext)
		: base(dbContext)
		{
		}

		protected override DbSet<AdminModel> DbSet => _dbContext.Admins;
	}
}
