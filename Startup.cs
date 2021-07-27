using HealthChecks.System;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TechStart.Core;
using TechStart.DbContexts;
using TechStart.HealthChecks;
using TechStart.Persistence;

namespace TechStart
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Allow CORS
            services.AddCors(options =>
            {
                options.AddPolicy("TechStartOrigin",
                      builder => builder.AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod());
            });

            #region Repositories
            services.AddScoped<IHospitalRepo, HospitalRepo>();
            services.AddScoped<IItemRepo, ItemRepo>();
            services.AddScoped<IItemVendorRepo, ItemVendorRepo>();
            services.AddScoped<IPharmacyRepo, PharmacyRepo>();
            services.AddScoped<IPharmacyInventoryRepo, PharmacyInventoryRepo>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<TechDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://auth.inkastudios.com";
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "inkapi";
                });

            #region Healthchecks
            services.AddHealthChecks()
                .AddSqlServer(Configuration["ConnectionStrings:DefaultConnection"])
                .AddDiskStorageHealthCheck(setup: delegate (DiskStorageOptions diskStorageOptions)
                    {
                        diskStorageOptions.AddDrive(@"D:\", minimumFreeMegabytes: 20000);
                    }, name: "Drive Health", HealthStatus.Unhealthy)
                .AddCheck<ResponseTimeHealthCheck>("Network speed test", null, new[] { "network" });
            #endregion

            // Singletons
            services.AddSingleton<ResponseTimeHealthCheck>();

            services.AddControllers();
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TechStart", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechStart v1"));
            }

            app.UseCors("TechStartOrigin");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapControllers();
            });
        }
    }
}
