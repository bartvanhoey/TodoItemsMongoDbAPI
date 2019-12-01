using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TodoItemsMongoDbAPI.DAL;

namespace TodoItemsMongoDbAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var environment = hostingContext.HostingEnvironment;

                    config.AddJsonFile(
                        "appsettings.json", optional: false, reloadOnChange: false);
                    config.AddJsonFile(
                        $"appsettings.{environment.EnvironmentName}.json", optional: false, reloadOnChange: false);
                    config.AddJsonFile(
                        $"secrets/appsettings.{environment.EnvironmentName}.json", optional: true,
                        reloadOnChange: false);

                    new MongoConnector(config).Init();

                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}