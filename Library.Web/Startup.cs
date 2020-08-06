using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Library.Service.Interfaces;
using Library.Service;
using Library.Data;
using Library.Data.Mapping;
using Library.Data.Models;
using Library.Service.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Library.Web {
    public class Startup {
        public Startup(IConfiguration config) {
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();

            services.AddSingleton(Configuration);
            
            services.AddDbContext<LibraryDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("LibraryConnection")));

            services.AddAutoMapper(
                c => c.AddProfile<EntityMappingProfile>(), typeof(Startup));
            
            services.AddScoped(typeof(IPaginator<Book>), typeof(Paginator<DbSet<Book>>));
            services.AddScoped(typeof(IPaginator<Video>), typeof(Paginator<DbSet<Video>>));
            services.AddScoped(typeof(IPaginator<LibraryAsset>), typeof(Paginator<DbSet<LibraryAsset>>));
            services.AddScoped(typeof(IPaginator<Patron>), typeof(Paginator<DbSet<Patron>>));
            services.AddScoped(typeof(IPaginator<LibraryBranch>), typeof(Paginator<DbSet<LibraryBranch>>));
            services.AddScoped(typeof(IPaginator<LibraryCard>), typeof(Paginator<DbSet<LibraryCard>>));
            services.AddScoped(typeof(IPaginator<Hold>), typeof(Paginator<DbSet<Hold>>));
            
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<IHoldService, HoldService>();
            services.AddScoped<ILibraryAssetService, LibraryAssetService>();
            services.AddScoped<ILibraryBranchService, LibraryBranchService>();
            services.AddScoped<ILibraryCardService, LibraryCardService>();
            services.AddScoped<IPatronService, PatronService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IVideoService, VideoService>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();

            app.UseEndpoints(routes => {
                routes.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}