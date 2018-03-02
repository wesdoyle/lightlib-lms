using LibraryData;
using LibraryService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Library
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton(Configuration);
            services.AddScoped<ILibraryCardService, LibraryCardServiceService>();
            services.AddScoped<ILibraryBranchService, LibraryBranchService>();
            services.AddScoped<IPatronService, PatronService>();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<ILibraryAssetService, LibraryAssetServiceService>();
            services.AddScoped<IBookService, BookServiceService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IStatusService, StatusServiceService>();

            services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LibraryConnection")));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}