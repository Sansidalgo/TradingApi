
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
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using NLog.Config;
using Microsoft.AspNetCore.Diagnostics;
using NLog.Extensions.Logging;
using System.Net.Http;
using sansidalgo.Server.Controllers.Trading;


namespace sansidalgo.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
           

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

           

            TimeZoneInfo indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Error);
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                loggingBuilder.AddNLog("nlog.config");
            });

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture("en-IN");
                options.AddSupportedCultures("en-IN");
                options.AddSupportedUICultures("en-IN");
                //options.SetDefaultCulture(TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            });
            builder.Services.AddRequestLocalization(options => { options.SetDefaultCulture("en-IN"); });
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

            var configuration = builder.Configuration;
            builder.Services.AddAutoMapper(typeof(BLU.MappingProfile));
            builder.Services.AddTransient<ShoonyaLogics>();
            builder.Services.AddSingleton<CommonHelper>(); // Register CommonHelper as a singleton
            builder.Services.AddScoped<ILogger<ShoonyaNewController>, Logger<ShoonyaNewController>>(); // Register ILogger for ShoonyaNewController
            builder.Services.AddScoped<AlgoContext>(); // Register AlgoContext as scoped (or singleton if needed)

            // Register repositories
            builder.Services.AddScoped<OrderSettingsRepository>();
            builder.Services.AddScoped<ShoonyaCredentialsRepository>();
            builder.Services.AddScoped<OrderRepository>();
            builder.Services.AddScoped<PlansRepository>();
            builder.Services.AddScoped<PaymentsRepository>();
            builder.Services.AddScoped<NseApiService>();
            builder.Services.AddTransient<AlgoContext>();
            builder.Services.AddTransient<UserSubscriptionRepository>();
            builder.Services.AddScoped<OptionsDataRepository>();


            builder.Services.AddDbContext<AlgoContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString"))
                   .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                   .EnableSensitiveDataLogging());
            builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });
            builder.Services.AddScoped<ILoginRepository, LoginRepository>();
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
            TimeSpan timeout = TimeSpan.FromSeconds(60);
            builder.Services.AddHttpClient<NseApiService>(client =>
            {
                client.Timeout = timeout;
                client.DefaultRequestHeaders.Add("User-Agent", "sansidalgo");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            
                client.BaseAddress = new Uri("https://www.nseindia.com/api/");

                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Add any required headers or settings here
            });
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

                    // Handle the exception, log it, etc.
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;

                    // Log or handle the exception as needed
                    // Example: Log the exception to the console
                    Console.WriteLine($"An unhandled exception occurred: {exception}");

                    // Set the response status code
                    context.Response.StatusCode = 500;

                    // Write a response message (customize as needed)
                    await context.Response.WriteAsync("An error occurred. Please try again later.");
                });
            });



            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
