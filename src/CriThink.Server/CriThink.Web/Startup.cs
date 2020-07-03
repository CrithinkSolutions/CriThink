using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using CriThink.Common.Endpoints;
using CriThink.Web.ActionFilters;
using CriThink.Web.Middlewares;
using CriThink.Web.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

// ReSharper disable UnusedMember.Global

namespace CriThink.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // ResponseCache
            SetupCache(services);

            // API versioning
            SetupAPIVersioning(services);

            // Swagger
            SetupSwagger(services);

            // ErrorHandling
            SetupErrorHandling(services);

            services
                .AddMvc(options => { options.EnableEndpointRouting = false; });
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Called by framework")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(); // Swagger
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                options.DisplayRequestDuration();
            });

            app.UseResponseCaching();

            app.UseApiVersioning();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!").ConfigureAwait(false);
                });
            });

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseMvc(); // Swagger
        }

        private static void SetupCache(IServiceCollection services)
        {
            services.AddResponseCaching();
            services.AddMemoryCache();
        }

        private static void SetupAPIVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true;
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ApiVersionReader = new HeaderApiVersionReader(EndpointConstants.ApiVersionHeader);
            });
        }

        private void SetupSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<AddRequiredHeaderParameter>();

                var contact = new OpenApiContact
                {
                    Name = Configuration["swaggerapiinfo:name"],
                    Email = Configuration["swaggerapiinfo:email"],
                    Url = new Uri(Configuration["SwaggerApiInfo:Uri"])
                };
                options.SwaggerDoc("v1", new OpenApiInfo { Title = $"{Configuration["SwaggerApiInfo:Title"]} v1", Version = "v1", Contact = contact });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                // Uses full schema names to avoid v1/v2/v3 schema collisions
                // see: https://github.com/domaindrivendev/Swashbuckle/issues/442
                //options.CustomSchemaIds(x => x.FullName);
            });
        }

        private static void SetupErrorHandling(IServiceCollection services)
        {
            services.AddScoped<ApiValidationFilterAttribute>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
