using System;
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
using Microsoft.Owin.Security.OAuth;
using MongoPersistence.Entities;
using MongoPersistence.Services;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using IdentityAPI;

namespace KennelAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IDogRepository, DogRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            //Create FakeEmailService
            services.AddTransient<IMailService, EmailService>();

            services.AddSwaggerGen(c =>
            { 
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "My First ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Talking Dotnet", Email = "contact@talkingdotnet.com", Url = "www.talkingdotnet.com" }
                });
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
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
                cfg.CreateMap<DogEntity, DogDto>();
                cfg.CreateMap<DogDtoCreation, DogEntity>();
                cfg.CreateMap<UserDtoCreation, UserEntity>();
            });

            

            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
