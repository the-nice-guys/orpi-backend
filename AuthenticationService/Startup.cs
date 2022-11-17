using AuthenticationService.Interfaces;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OrpiLibrary;
using OrpiLibrary.Models;
using OrpiLibrary.Interfaces;

namespace AuthenticationService {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            var accessTokenData = new AccessTokenData();
            var refreshTokenData = new RefreshTokenData();
            services.AddMvc();
            services.AddTransient<ITokenCreator, TokenCreator>();
            services.AddTransient<ICryptographer, Cryptographer>();
            services.AddSingleton<IUsersWorker>(provider => new DatabaseService(
                host: Config.Host,
                port: Config.Port,
                database: Config.Database,
                user: Config.User,
                password: Config.Password
                )
            );
            services.AddSingleton(provider => new TokenDataManager(accessTokenData, refreshTokenData));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = refreshTokenData.GetTokenValidationParameters();
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors(builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => {
                 endpoints.MapControllers();
            });
        }
    }
}