using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Interfaces;
using Common.Interfaces.Services;
using KennelAPI.Models;
using KennelAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoPersistence.Entities;
using MongoPersistence.Services;
using NJsonSchema;

namespace KennelAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerDocument(c =>
            {
                c.Version = "2.0";
                c.Title = "KennelAPI";
            });
            services.AddSingleton<IDogRepository, InMemoryDogRepository>();
            services.AddTransient<IMailService, EmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler("/Error");
            //app.UseStatusCodePagesWithReExecute("/Error");

            /*app.Use((context, next) =>
            {
                // Do work that doesn't write to the Response.
                //await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
                //throw new Exception();
                //await context.Response.WriteAsync("Hello 2!");

                return next();
            });*/

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DogDtoCreation, DogEntity>();
            });

            app.UseSwaggerUi3(settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
            });
            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
