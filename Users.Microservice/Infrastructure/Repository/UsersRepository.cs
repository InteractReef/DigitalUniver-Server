using InteractReef.Database.Core;
using InteractReef.Packets.Users;
using Microsoft.EntityFrameworkCore;
using Users.Microservice.Infrastructure.Database;

namespace Users.Microservice.Infrastructure.Repository
{
	public class UsersRepository : GenericRepository<UsersDbContext, UserModel>
	{
		public UsersRepository(UsersDbContext dbContext)
		: base(dbContext)
		{
		}


		protected override DbSet<UserModel> DbSet => _dbContext.Users;
	}
}
