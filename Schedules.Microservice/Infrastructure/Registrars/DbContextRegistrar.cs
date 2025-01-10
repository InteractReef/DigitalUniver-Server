using InteractReef.Database.Core;
using InteractReef.Packets;
using Microsoft.EntityFrameworkCore;
using Schedules.Microservice.Infrastructure.Database;
using Schedules.Microservice.Infrastructure.Database.Repository;

namespace Schedules.Microservice.Infrastructure.Registrars
{
	public static class DbContextExtensions
	{
		public static async Task MigrateDatabaseAsync<TContext>(this IHost webApp) where TContext : DbContext
		{
			await using var scope = webApp.Services.CreateAsyncScope();
			await using var appContext = scope.ServiceProvider.GetRequiredService<TContext>();

			await appContext.Database.MigrateAsync();
		}
	}

	public static class DbContextRegistrar
	{
		private const string ConnectionStringName = "DefaultConnection";

		public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString(ConnectionStringName);

			services.AddDbContext<SchedulesDbContext>(opts => opts.UseNpgsql(connectionString));

			services.AddScoped<IRepository<ScheduleItem>, ScheduleItemsRepository>();
			services.AddScoped<IRepository<Schedule>, SchedulesRepository>();
			services.AddScoped<IRepository<SubjectItem>, SubjectsRepository>();

			return services;
		}
	}
}
