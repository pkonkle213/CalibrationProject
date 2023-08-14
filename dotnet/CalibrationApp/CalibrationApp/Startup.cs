using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using CalibrationApp.DAO;
using CalibrationApp.Security;

namespace CalibrationApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Adds swagger documentation file support. See https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio for more details
            ConfigureSwagger(services);

            // Add CORS policy allowing any origin
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
                                                                  .AllowAnyMethod()
                                                                  .AllowAnyHeader());
            });

            // configure jwt authentication
            byte[] key = Encoding.ASCII.GetBytes(Configuration["JwtSecret"]);
            ConfigureJwtAuthentication(services, key);

            // Dependency Injection configuration.
            // Non-testing settings will be pulled from the secure environment when hosted on a server
            services.AddSingleton<ITokenGenerator>(sp => new JwtGenerator(Configuration["JwtSecret"]));
            services.AddSingleton<IPasswordHasher>(sp => new PasswordHasher());

            string connectionString = Configuration.GetConnectionString("Project");
            services.AddTransient<IUserDAO>(sp => new SqlUserDAO(connectionString));
            services.AddTransient<IIndividualDAO>(sp => new SqlIndividualDAO(connectionString));
            services.AddTransient<IGroupDAO>(sp => new SqlGroupDAO(connectionString));
            services.AddTransient<IQuestionDAO>(sp => new SqlQuestionDAO(connectionString));
            services.AddTransient<IStatsDAO>(sp => new SqlStatsDAO(connectionString));
            services.AddTransient<ICalibrationDAO>(sp => new SqlCalibrationDAO(connectionString));
            services.AddTransient<IFormDAO>(sp => new SqlFormDAO(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Adds swagger documentation file. See https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio for more details
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                s.RoutePrefix = string.Empty;
            });

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Customize me in Startup.cs",
                    Description = "Or just leave it and hope no hiring manager notices..."
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);
            });
        }

        private static void ConfigureJwtAuthentication(IServiceCollection services, byte[] key)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[JwtRegisteredClaimNames.Sub] = "sub";

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    NameClaimType = "name"
                };
            });
        }
    }
}
