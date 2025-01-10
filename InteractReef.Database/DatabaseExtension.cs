using InteractReef.Database.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace InteractReef.Database.Core
{
	public static class DatabaseExtension
	{
		public static async Task<TContext> GetDbContext<TContext>(this IHost webApp) where TContext : DbContext
		{
			await using var scope = webApp.Services.CreateAsyncScope();
			await using var appContext = scope.ServiceProvider.GetRequiredService<TContext>();

			return appContext;
		}
	}
}
