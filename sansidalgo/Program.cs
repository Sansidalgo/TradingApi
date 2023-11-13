using NLog;
using sansidalgo.core.helpers;
using sansidalgo.core.Vendors;
using sansidalgo.core.Vendors.Interfaces;
using System.Reflection.PortableExecutable;

namespace sansidalgo
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


        
            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
           builder.Services.AddTransient<CommonHelper, CommonHelper>();
            builder.Services.AddTransient<ShoonyaLogics>();
            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
          

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}