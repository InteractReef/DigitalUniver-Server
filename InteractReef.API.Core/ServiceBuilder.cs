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

			var serviceName = Assembly.GetCallingAssembly().GetName().Name.Replace(".Microservice", "");

			builder.Services.Configure<ServiceSetting>(opt => builder.Configuration.GetSection($"Services:{serviceName}"));
			builder.Services.Configure<DatabaseConnectionData>(opt =>
			{
				opt.ConnectionString = builder.Configuration.GetConnectionString(serviceName); 
			});

			return builder;
		}

		public static WebApplicationBuilder ConfigurePorts(this WebApplicationBuilder builder)
		{
			var serviceName = Assembly.GetCallingAssembly().GetName().Name.Replace(".Microservice", "");

			builder.WebHost.ConfigureKestrel((context, x) =>
			{
				var serviceSetting = builder.Configuration.GetSection($"Services:{serviceName}").Get<ServiceSetting>();

				x.ListenAnyIP(serviceSetting.Ports[0], option =>
				{
					option.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
				}); // Open API

				x.Listen(System.Net.IPAddress.Parse("0.0.0.0"), serviceSetting.Ports[1], option =>
				{
					option.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
				}); // Grpc
			});

			return builder;
		}

		public static IServiceCollection AddCorsPolicy(this IServiceCollection service)
		{
			service.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigin", policy =>
				{
					policy.WithOrigins("http://localhost:3000")
						  .AllowAnyHeader()
						  .AllowAnyMethod();
				});
			});

			return service;
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
	}
}
