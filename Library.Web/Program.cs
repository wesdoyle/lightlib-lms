using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Library.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
	        CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
	        WebHost.CreateDefaultBuilder(args).UseStartup<Startup>()
				.ConfigureLogging((hostingContext, logging) => {
					logging.AddConsole()
						.AddFilter<ConsoleLoggerProvider>(category: null, level: LogLevel.Information);
				});
	}
}