using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Library.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
	        CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                webBuilder.UseStartup<Startup>()
                    .UseUrls("https://*:8001", "http://*:8000");
            });
    }
}