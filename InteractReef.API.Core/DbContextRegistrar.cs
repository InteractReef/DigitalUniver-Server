using InteractReef.Database.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InteractReef.API.Core
{
	public static class DbContextRegistrar
	{
		public static IServiceCollection AddDatabaseContext<T>(this IServiceCollection services, IConfiguration configuration, Action<DbContextOptionsBuilder> dbContextOptionsAction) where T : DbContext
		{
			var name = Assembly.GetCallingAssembly().GetName().Name.Replace(".Microservice", "");
			var connectionString = configuration.GetConnectionString(name);

			services.AddDbContext<T>(opt =>
			{
				dbContextOptionsAction?.Invoke(opt);
			});

			return services;
		}
	}
}
