using Microsoft.EntityFrameworkCore;
using InteractReef.API.Core;
using InteractReef.Database.Core;
using Organizations.Microservice.Infrastructure.Database;
using InteractReef.Packets.Organizations;
using Organizations.Microservice.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration(args);
builder.ConfigurePorts();

builder.Services.AddSequrity(builder.Configuration);
builder.Services.AddDatabase<OrganizationsDbContext>(builder.Configuration, (config, option) =>
{
	option.UseNpgsql(config.ConnectionString);
});

//builder.Services.AddRepository();
builder.Services.AddScoped<IRepository<EmployeeModel>, EmployeesRepository>();
builder.Services.AddScoped<IRepository<OrganizationModel>, OrganizationsRepository>();
builder.Services.AddScoped<IRepository<StudentModel>, StudentsRepository>();

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

app.Run();
