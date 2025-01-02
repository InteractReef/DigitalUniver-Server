using Identity.Microservice.Infrastructure.Database;
using InteractReef.Database.Core;
using Microsoft.EntityFrameworkCore;

namespace Identity.Microservice.Infrastructure.Services
{
	public static class DbContextRegistrar
	{
		private const string ConnectionStringName = "EmployeesDB";

		public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString(ConnectionStringName);

			services.AddDbContext<UsersDbContext>(opts => opts.UseNpgsql(connectionString));

			services.AddScoped<IRepository<UserModel>, UsersRepository>();

			return services;
		}
	}
}
