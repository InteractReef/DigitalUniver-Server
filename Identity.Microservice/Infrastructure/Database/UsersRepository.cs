using InteractReef.Database.Core;
using Microsoft.EntityFrameworkCore;

namespace Identity.Microservice.Infrastructure.Database
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
