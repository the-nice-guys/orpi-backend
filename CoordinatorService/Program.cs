using Confluent.Kafka;
using coordinator_service.Interfaces;
using coordinator_service.Services;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisCache");
    options.InstanceName = "master";
});

builder.Services.AddSingleton<IHostedService, ConsumerService>(provider => 
    new ConsumerService(provider.GetService(typeof(IDistributedCache)) as IDistributedCache, builder.Configuration["Kafka:BootstrapServers"], builder.Configuration["Kafka:ResponseTopic"]));
builder.Services.AddSingleton<IProducerService, ProducerService>(provider => new ProducerService(builder.Configuration["Kafka:BootstrapServers"]));
builder.Services.AddSingleton<IDeploymentService, DeploymentService>();
builder.Services.AddSingleton<IStartService, StartService>();


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