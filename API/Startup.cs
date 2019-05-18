using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.DependencyInjection;
using Models.Tags.Repositories;
using Swashbuckle.AspNetCore.Swagger;

namespace API
{
    public class Startup
    {
        private const string DocsRoute = "secret-materials/docs";
        private const string DocName = "SH-API";
        private const string DocTitle = "StopHram API";
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITagRepository, MongoTagRepository>();
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAnyPolicy"));
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyPolicy", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            services.AddMvc();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(DocName, new Info
                {
                    Version = "v1",
                    Title = DocTitle,
                    Description = "ASP.NET Core Web API"
                });
                options.IncludeXmlComments($"{AppDomain.CurrentDomain.BaseDirectory}/API.xml");
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStatusCodePagesWithReExecute("/index.html");

            //todo запретить cors после завершения разработки
            app.UseCors("AllowAnyPolicy");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();

            app.UseSwagger(options => options.RouteTemplate = $"{DocsRoute}/{{documentName}}/swagger.json");
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = DocsRoute;
                options.SwaggerEndpoint($"/{DocsRoute}/{DocName}/swagger.json", DocTitle);
            });
        }
    }
}