using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonitoringModule.Hubs;
using MonitoringModule.Interfaces;
using MonitoringModule.Services;

namespace MonitoringModule; 

public class Startup {
    public void ConfigureServices(IServiceCollection services) {
        services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials();
            })
        );
        services.AddControllers();
        services.AddSignalR();
        services.AddSingleton<IMonitoringService, MonitoringService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseCors("CorsPolicy");

        app.UseRouting();

        app.UseEndpoints(endpoints => {
             endpoints.MapControllers();
             endpoints.MapHub<MonitoringHub>("monitoring/connect");
        });
    }
}