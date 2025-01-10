using Microsoft.EntityFrameworkCore;
using InteractReef.API.Core;
using InteractReef.Database.Core;
using Schedules.Microservice.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration(args);
builder.ConfigurePorts();

builder.Services.AddSequrity(builder.Configuration);
builder.Services.AddDatabase<SchedulesDbContext>(builder.Configuration, (config, option) =>
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

var context = await app.GetDbContext<SchedulesDbContext>();
await context.Database.MigrateAsync();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
