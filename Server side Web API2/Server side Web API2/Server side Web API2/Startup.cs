using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using A2.Model;
using A2.Data;
using A2.Handler;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace A2
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
            services.AddDbContext<DataBase>(options => options.UseSqlite(Configuration.GetConnectionString("AuthDbConnection")));
            services.AddControllers();
            services.AddScoped<IDatabaseRepo, DatabaseRepo>();
            services.AddAuthentication().//register authentication scheme
            AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>
            ("MyAuthentication", null);
            services.AddAuthorization(options =>//registers a authorization policy/policies, parameter is a delegate
            {
                options.AddPolicy("UserOnly", policy => policy.RequireClaim("UserName"));// parameter 1 is the name of policy called "UserOnly", parameter 2 is to check if the clients claim has the key called "UserName"
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "A2", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "A2 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();// need to have this autentication component in the processing pipeline between routing and authorization   
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
