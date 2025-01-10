using InteractReef.API.Core;
using Identity.Microservice.Infrastructure.Channels;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration(args);
builder.ConfigurePorts();

builder.Services.AddSequrity(builder.Configuration);
builder.Services.AddRepository();

builder.Services.AddSingleton<UserChannel>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
