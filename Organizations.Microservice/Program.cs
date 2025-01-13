using Microsoft.EntityFrameworkCore;
using InteractReef.API.Core;
using InteractReef.Database.Core;
using Organizations.Microservice.Infrastructure.Database;
using InteractReef.Packets.Organizations;
using Organizations.Microservice.Infrastructure.Repository;
using Organizations.Microservice.Infrastructure.GrpcService;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration(args);
builder.ConfigurePorts();

builder.Services.AddSequrity(builder.Configuration);
builder.Services.AddDatabase<OrganizationsDbContext>(builder.Configuration, (config, option) =>
{
	option.UseNpgsql(config.ConnectionString);
});

builder.Services.AddScoped<IRepository<OrganizationModel>, OrganizationsRepository>();

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

await app.Services.GetRequiredService<OrganizationsDbContext>().Database.MigrateAsync();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<OrganizationService>();

app.Run();
