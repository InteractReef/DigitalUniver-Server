using Users.Microservice.Infrastructure.Services;
using Users.Microservice.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using InteractReef.API.Core;
using InteractReef.Database.Core;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration(args);
builder.ConfigurePorts();

builder.Services.AddSequrity(builder.Configuration);
builder.Services.AddDatabase<UsersDbContext>(builder.Configuration, (config, option) =>
{
	option.UseNpgsql(config.ConnectionString);
});

builder.Services.AddRepository();

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

var context = await app.GetDbContext<UsersDbContext>();
await context.Database.MigrateAsync();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<AuthUserService>();

app.Run();
