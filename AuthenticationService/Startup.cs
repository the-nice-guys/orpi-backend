using AuthenticationService.Extensions;
using AuthenticationService.Interfaces;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrpiLibrary.Interfaces.Tokens;
using OrpiLibrary.Models.Common.Tokens;

namespace AuthenticationService {
    public class Startup {
        public Startup(IConfiguration configuration) {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services) {
            var accessTokenData = new AccessTokenData();
            var refreshTokenData = new RefreshTokenData();
            
            services.AddMvc();
            services.AddTransient<ITokenCreator, TokenCreator>();
            services.AddTransient<ICryptographer, Cryptographer>();
            services.AddSingleton<IUsersWorker>(new DatabaseService(
                _configuration["UserDataBase:Host"],
                _configuration["UserDataBase:Port"],
                _configuration["UserDataBase:Name"],
                _configuration["UserDataBase:User"],
                _configuration["UserDataBase:Password"]
                )
            );
            
            services.AddSingleton(new TokenDataManager(accessTokenData, refreshTokenData));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = refreshTokenData.ValidationParameters;
            });
            
            DatabaseExtension.SetConnectionString(_configuration);
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

        private readonly IConfiguration _configuration;
    }
}