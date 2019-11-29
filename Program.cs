using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TodoItemsMongoDbAPI.DAL;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace TodoItemsMongoDbAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == EnvironmentName.Development;


            CreateHostBuilder(args).Build().Run();


//                .MigrateDatabase()
            ;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile(
                        "appsettings.json", optional: false, reloadOnChange: false);
                    config.AddJsonFile(
                        "appsettings.Development.json", optional: false, reloadOnChange: false);
                    config.AddJsonFile(
                        "appsettings.Docker.json", optional: false, reloadOnChange: false);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.MigrateDatabase();

                    webBuilder.UseStartup<Startup>();
                });
    }
}