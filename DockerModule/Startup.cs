using DockerModule.Interfaces;
using DockerModule.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Unity;

namespace DockerModule;

public class Startup {
    public Startup() {
        _container = new UnityContainer();
    }
    
    public void ConfigureServices(IServiceCollection services) {
        RegisterDependencies();
        services.AddControllers();
        services.AddHostedService(_ => _container.Resolve<KafkaConsumerHostedService>());
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

    private readonly UnityContainer _container;

    private void RegisterDependencies() {
        _container.RegisterType<IResponder, KafkaResponder>();
        _container.RegisterType<IServiceManager, DockerServiceManager>();
    }
}