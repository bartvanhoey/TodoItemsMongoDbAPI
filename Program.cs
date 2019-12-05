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
                   var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                    config.AddJsonFile(
                        $"secrets/appsettings.{environmentName}.json", optional: true);

                    new MongoConnector(config).Init();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}