
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
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<ShoonyaLogics>();
            builder.Services.AddTransient<CommonHelper>();
            builder.Services.AddTransient<AlgoContext>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "SansidAlgo",
        ValidAudience = "SansidAlgoAudience",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("01B718E1348642199422B0D8DBC0A6BD"))
    };
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

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
