
using BLU.Repositories.Interfaces;
using BLU.Repositories;
using DataLayer.Models;
using NLog;
using sansidalgo.core.helpers;
using sansidalgo.core.Vendors;
using sansidalgo.core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BLU.Services;
using Microsoft.OpenApi.Models;
using sansidalgo.Server.Models;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace sansidalgo.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfiguration(builder =>
            {
                //builder.ForLogger().FilterMinLevel(NLog.LogLevel.Info).WriteTo.Console();
                builder.ForLogger().FilterMinLevel(NLog.LogLevel.Info).WriteToFile(fileName: "logs/log.txt");
                builder.ForLogger().FilterMinLevel(NLog.LogLevel.Error).WriteToFile(fileName: "logs/log.txt");
                builder.ForLogger().FilterMinLevel(NLog.LogLevel.Trace).WriteToFile(fileName: "logs/log.txt");
            }).GetCurrentClassLogger();
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                // Add the security definition for JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                // Add the JWT token authorization requirement
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                // ... other Swagger configuration
            });
            builder.Services.AddTransient<ShoonyaLogics>();
            builder.Services.AddTransient<CommonHelper>();
            builder.Services.AddTransient<AlgoContext>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                   
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            builder.Services.AddSingleton<IRefreshTokenGenerator>(provider => new RefreshTokenGenerator());
            // Access configuration settings
            var configuration = builder.Configuration;
            var jwtSettings = configuration.GetSection("JWTSetting");
            builder.Services.Configure<JWTSetting>(jwtSettings);

            var authkey = configuration.GetValue<string>("JWTSetting:securitykey");

            
            builder.Services.AddAuthentication(item =>
            {
                item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(item =>
            {

                item.RequireHttpsMetadata = true;
                item.SaveToken = true;
                item.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authkey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Register JwtService
           builder.Services.AddScoped<JwtService>(provider => new JwtService(authkey));

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    // Add CORS header to allow error message to be visible to Angular
                    if (context.Request.Headers.TryGetValue("Origin", out StringValues origin))
                    {
                        context.Response.Headers.Add("Access-Control-Allow-Origin", origin.ToString());
                    }
                });
            });

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}