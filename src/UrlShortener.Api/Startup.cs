using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UrlShortener.Persistence.Contracts;
using UrlShortener.Persistence.Implementation.SQLServer;
using UrlShortener.Services.Contracts;
using UrlShortener.Services.Implementation;

namespace UrlShortener.Api
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            if (Configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<DataContext>(options =>
                     options.UseInMemoryDatabase("UrlShortenerDb"));
            }
            else
            {
                services.AddDbContext<DataContext>( options =>
                     options.UseSqlServer(
                         Configuration.GetConnectionString("DefaultConnection"),
                         b => b.MigrationsAssembly(typeof(DataContext).Assembly.FullName)));
            }

            services.AddControllers();
            services.AddMemoryCache();

            services.AddTransient<IUrlRepository, UrlRepository>();
            services.AddTransient<IUrlService, UrlService>();
            services.AddTransient<ICacheService, MemoryCacheService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints( endpoints =>
             {
                 endpoints.MapControllers();
             } );
        }
    }
}
