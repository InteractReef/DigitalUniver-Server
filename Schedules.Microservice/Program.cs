using InteractReef.Sequrity;
using Schedules.Microservice.Infrastructure.Database;
using Schedules.Microservice.Infrastructure.Registrars;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(x =>
{
	x.ListenLocalhost(5005, option =>
	{
		option.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
	}); // Grpc

	x.ListenAnyIP(5004); // Open API
});

var configuration = builder.Configuration;

builder.Services.AddDbContext(configuration);
builder.Services.AddJwt(configuration);
builder.Services.AddSingleton<ITokenController, TokenController>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

await app.MigrateDatabaseAsync<SchedulesDbContext>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
