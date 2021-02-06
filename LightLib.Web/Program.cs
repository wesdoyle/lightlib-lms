using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace LightLib.Web {
    public static class Program {
        public static void Main(string[] args) {
            var logger = NLog.Web.NLogBuilder
                .ConfigureNLog("nlog.config")
                .GetCurrentClassLogger();
            try {
                CreateHostBuilder(args).Build().Run();
            } catch (Exception e) {
                //NLog: catch setup errors
                logger.Error(e, "Stopped program because of exception");
                throw; 
            } finally {
                // Ensure to flush and stop internal timers/threads before
                // application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        } 

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                webBuilder.UseStartup<Startup>()
                    .UseUrls("https://*:8001", "http://*:8000")
                    .ConfigureLogging(logging => {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    })
                    .UseNLog();

            });
    }
}