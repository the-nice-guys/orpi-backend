using AuthenticationService.Interfaces;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrpiLibrary;
using OrpiLibrary.Interfaces.Tokens;
using OrpiLibrary.Models.Common.Tokens;

namespace AuthenticationService {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            var accessTokenData = new AccessTokenData();
            var refreshTokenData = new RefreshTokenData();
            services.AddMvc();
            services.AddTransient<ITokenCreator, TokenCreator>();
            services.AddTransient<ICryptographer, Cryptographer>();
            services.AddSingleton<IUsersWorker>(new DatabaseService(
                Config.AuthenticationServiceDatabaseHost,
                Config.AuthenticationServiceDatabasePort,
                Config.AuthenticationServiceDatabaseName,
                Config.AuthenticationServiceDatabaseUser,
                Config.AuthenticationServiceDatabasePassword
                )
            );
            services.AddSingleton(new TokenDataManager(accessTokenData, refreshTokenData));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = refreshTokenData.ValidationParameters;
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