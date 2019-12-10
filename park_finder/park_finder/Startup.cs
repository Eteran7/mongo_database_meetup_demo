using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;

using ParkFinder.Services;

namespace ParkFinder
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IMongoClient, MongoClient>(client => new MongoClient("mongodb://user:pass@localhost:8000/meetup"));

            // Configure services.
            services.AddScoped<IParkService, ParkService>();

            // Configure swagger generation.
            services.AddSwaggerGen(generation =>
            {
                generation.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Park data service"
                });

                generation.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Parks Service";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parks Service");
                c.RoutePrefix = "api/swagger";
            });
        }
    }
}
