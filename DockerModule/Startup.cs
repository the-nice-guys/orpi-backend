using DockerModule.Interfaces;
using DockerModule.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Unity;

namespace DockerModule;

public class Startup {

    public void ConfigureServices(IServiceCollection services) {
        services.AddScoped<IResponder, KafkaResponder>();
        services.AddScoped<IServiceManager, DockerServiceManager>();
        services.AddHostedService<KafkaConsumerHostedService>();
        services.AddControllers();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseCors(builder => {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
        
        app.UseRouting();

        app.UseEndpoints(endpoints => {
             endpoints.MapControllers();
        });
    }
}