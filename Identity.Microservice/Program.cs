using Identity.Microservice.Infrastructure.Channels;
using InteractReef.Sequrity;

namespace Identity.Microservice
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.WebHost.ConfigureKestrel(x =>
			{
				x.ListenLocalhost(5001, options =>
				{
					options.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
				});
				x.ListenAnyIP(5000);
			});

			var configuration = builder.Configuration;

			builder.Services.AddSingleton<UserChannel>();
			builder.Services.AddSingleton<ITokenController, TokenController>();

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.MapControllers();

			app.Run();
		}
	}
}
