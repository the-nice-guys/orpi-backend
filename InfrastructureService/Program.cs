using infrastructure_service.Interfaces;
using OrpiLibrary.Models;
using infrastructure_service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OrpiLibrary.Models.Common.Tokens;

var builder = WebApplication.CreateBuilder(args);
string dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
//string connectionString = $"host={dbHost};port=5432;database=infrastructure-service;username=postgres;password=postgres";
// add connection string to the configuration
//builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

// Add services to the container.

builder.Services.AddSingleton<IInfrastructureRepository, InfrastructureRepository>(provider => new InfrastructureRepository(builder.Configuration));
builder.Services.AddSingleton<IHostRepository, HostRepository>(provider => new HostRepository(builder.Configuration));
builder.Services.AddSingleton<IServiceRepository, ServiceRepository>(provider => new ServiceRepository(builder.Configuration));
builder.Services.AddSingleton<IOptionsRepository, OptionsRepository>(provider => new OptionsRepository(builder.Configuration));
//builder.Services.AddSingleton<IHostedService, ConsumerService>(provider => new ConsumerService((IInfrastructureRepository) provider.GetService(typeof(InfrastructureRepository)), "massages"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new AccessTokenData().ValidationParameters;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// test 2

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();