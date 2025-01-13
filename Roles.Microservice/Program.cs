using Microsoft.EntityFrameworkCore;
using InteractReef.API.Core;
using InteractReef.Database.Core;
using Roles.Microservice.Infrastructure.Database;
using InteractReef.Packets;
using Roles.Microservice.Infrastructure.Repository;
using Roles.Microservice.Infrastructure.GrpcService;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration(args);
builder.ConfigurePorts();

builder.Services.AddSequrity(builder.Configuration);
builder.Services.AddDatabase<RolesDbContext>(builder.Configuration, (config, option) =>
{
	option.UseNpgsql(config.ConnectionString);
});

builder.Services.AddScoped<IRepository<AdminModel>, AdminsRepository>();
builder.Services.AddScoped<IRepository<EmployeeModel>, EmployeesRepository>();
builder.Services.AddScoped<IRepository<StudentModel>, StudentsRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

await app.Services.GetRequiredService<RolesDbContext>().Database.MigrateAsync();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<RolesService>();

app.Run();
