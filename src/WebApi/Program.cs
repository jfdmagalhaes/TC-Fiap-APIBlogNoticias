using Infraestructure;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;


namespace WebApi
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseUrls("http://0.0.0.0:8999", "https://0.0.0.0:8998");

                });
    }
}