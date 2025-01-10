using InteractReef.Database.Core;
using InteractReef.Sequrity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace InteractReef.API.Core
{
	public static partial class ServiceBuilder
	{
		public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder, string[] args)
		{
			builder.Configuration
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("AppSettings-global.json", optional: true, reloadOnChange: true)
				.AddEnvironmentVariables()
				.AddCommandLine(args);

			var serviceName = Assembly.GetExecutingAssembly().GetName().Name.Replace(".Microservice", "");

			builder.Services.Configure<ServiceSetting>(opt => builder.Configuration.GetSection(serviceName));
			builder.Services.Configure<DatabaseConnectionData>(opt =>
			{
				var connectionString = builder.Configuration.GetConnectionString(serviceName);
				opt.ConnectionString = connectionString; 
			});

			return builder;
		}

		public static WebApplicationBuilder ConfigurePorts(this WebApplicationBuilder builder)
		{
			var serviceName = Assembly.GetExecutingAssembly().GetName().Name.Replace(".Microservice", "");

			builder.WebHost.ConfigureKestrel((context, x) =>
			{
				var serviceSetting = builder.Configuration.GetSection(serviceName).Get<ServiceSetting>();

				x.ListenAnyIP(serviceSetting.Ports[0], option =>
				{
					option.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
				}); // Open API

				x.ListenLocalhost(serviceSetting.Ports[1], option =>
				{
					option.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
				}); // Grpc
			});

			return builder;
		}

		public static IServiceCollection AddSequrity(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddJwtValidator(configuration);
			services.AddJwt();
			return services;
		}

		public static IServiceCollection AddDatabase<T>(this IServiceCollection services, IConfiguration configuration, Action<DatabaseConnectionData, DbContextOptionsBuilder> dbContextOptions) where T : DbContext
		{
			var serviceProvider = services.BuildServiceProvider();
			var databaseConfig = serviceProvider.GetRequiredService<IOptions<DatabaseConnectionData>>().Value;
			services.AddDbContext<T>(opt =>
			{
				dbContextOptions?.Invoke(databaseConfig, opt);
			});
			return services;
		}

		public static IServiceCollection AddRepository(this IServiceCollection services)
		{
			var repositoryTypes = Assembly.GetCallingAssembly().GetTypes()
				.Where(t => t.IsSubclassOf(typeof(GenericRepository<,>)) && !t.IsAbstract)
				.ToList();

			foreach (var type in repositoryTypes)
			{
				var dbSetProperty = type.GetProperties()
					.FirstOrDefault(p => typeof(DbSet<>).IsAssignableFrom(p.PropertyType));

				if (dbSetProperty != null)
				{
					var entityType = dbSetProperty.PropertyType.GetGenericArguments().First();

					var repositoryInterface = typeof(IRepository<>).MakeGenericType(entityType);
					var repositoryType = typeof(GenericRepository<,>).MakeGenericType(type, entityType);

					services.AddScoped(repositoryInterface, repositoryType);
				}
			}

			return services;
		}
	}
}
