using Confluent.Kafka;
using coordinator_service.Interfaces;
using coordinator_service.Services;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "master";
});

builder.Services.AddSingleton<IHostedService, ConsumerService>(provider => 
    new ConsumerService(provider.GetService(typeof(IDistributedCache)) as IDistributedCache, "docker-responses"));
builder.Services.AddSingleton<IProducerService, ProducerService>(provider => new ProducerService());
builder.Services.AddSingleton<IDeploymentService, DeploymentService>();


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();