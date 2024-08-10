using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Email;

namespace BrainstormSessions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AddLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void AddLogger()
        {
            var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            var logFileName = $"log_{DateTime.Now:yyyyMMddHHmmss}.txt";
            var logFilePath = Path.Combine(logDirectory, logFileName);

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.File(logFilePath)
                .WriteTo.Email(new EmailSinkOptions
                {
                    From= "your gmail",
                    To = { "your gmail" },
                    Host = "smtp.gmail.com",
                    Credentials = new NetworkCredential("your gmail", "your password"),
                    Port = 465
                },
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .CreateLogger();
        }
    }
}
