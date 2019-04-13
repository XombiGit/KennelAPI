﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Common.Interfaces;
using Common.Interfaces.Services;
using KennelAPI.Models;
using KennelAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoPersistence.Entities;
using MongoPersistence.Services;
using Swashbuckle.Application;

namespace KennelAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IDogRepository, DogRepository>();
            //Create FakeEmailService
            services.AddTransient<IMailService, EmailService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //ValidateIssuer = true,
                        //ValidateAudience = true,
                        //ValidateLifetime = true,
                        //ValidateIssuerSigningKey = true,
                        //ValidIssuer = "",
                        //ValidAudience = "",
                        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(""))
                    };
                });


            //GlobalConfiguration.Configuration
  //.EnableSwagger(c => c.SingleApiVersion("v1", "A title for your API"))
  //.EnableSwaggerUi();
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



            app.UseAuthentication();
            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
