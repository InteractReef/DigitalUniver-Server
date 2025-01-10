using InteractReef.Database.Core;
using InteractReef.Packets.User;
using Microsoft.EntityFrameworkCore;

namespace Users.Microservice.Infrastructure.Database
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
