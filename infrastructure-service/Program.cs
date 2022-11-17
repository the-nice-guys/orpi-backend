using infrastructure_service.Interfaces;
using infrastructure_service.Models;
using infrastructure_service.Services;

var builder = WebApplication.CreateBuilder(args);
string connectionString = "host=localhost;port=5432;database=infrastructure-service;username=postgres;password=postgres";
// Add services to the container.

builder.Services.AddSingleton<IInfrastructureRepository, InfrastructureRepository>(provider => new InfrastructureRepository(connectionString));
// builder.Services.AddSingleton<IHostedService, ConsumerService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// test 2


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();