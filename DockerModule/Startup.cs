using DockerModule.Interfaces;
using DockerModule.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Unity;

namespace DockerModule;

public class Startup
{
    private readonly WebHostBuilderContext _webHostBuilderContext;

    public Startup(WebHostBuilderContext webHostBuilderContext)
    {
        _webHostBuilderContext = webHostBuilderContext;
    }

    public void ConfigureServices(IServiceCollection services) {
        services.AddScoped<IResponder, KafkaResponder>();
        services.AddSingleton<IUpdateRepository, UpdateRepository>();
        services
            .AddOptions<UpdateRepositoryConfig>()
            .Bind(_webHostBuilderContext.Configuration.GetSection(nameof(UpdateRepositoryConfig)));
 
        services.AddControllers();
        
        services.AddHostedService<KafkaConsumerHostedService>();
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
