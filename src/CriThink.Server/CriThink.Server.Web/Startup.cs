using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints;
using CriThink.Server.Core.Entities;
using CriThink.Server.Infrastructure;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.Repositories;
using CriThink.Server.Providers.DomainAnalyzer;
using CriThink.Server.Providers.EmailSender;
using CriThink.Server.Providers.EmailSender.Settings;
using CriThink.Server.Providers.NewsAnalyzer;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Facades;
using CriThink.Server.Web.Middlewares;
using CriThink.Server.Web.Services;
using CriThink.Server.Web.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

// ReSharper disable UnusedMember.Global

namespace CriThink.Server.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Database
            services.AddDbContext<CriThinkDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("CriThinkDbSqlConnection");
                options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            });

            // User Identity
            SetupUserIdentity(services);

            // Jwt
            SetupJwtAuthentication(services);

            // ResponseCache
            SetupCache(services);

            // API versioning
            SetupAPIVersioning(services);

            // Swagger
            SetupSwagger(services);

            // Settings
            SetupSettings(services);

            // MediatR
            services.AddMediatR(typeof(Startup), typeof(Bootstrapper));

            // Internal
            SetupInternalServices(services);

            // ErrorHandling
            SetupErrorHandling(services);

            // Gzip
            services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });

            services
                .AddMvc(options => { options.EnableEndpointRouting = false; })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new NewsSourceClassificationConverter());
                });

            services.AddAutoMapper(typeof(Startup)); // AutoMapper

            services.AddRazorPages(); // Razor
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Called by framework")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger(); // Swagger
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    options.DisplayRequestDuration();
                });
            }

            app.UseResponseCaching();

            app.UseApiVersioning();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Identity

            app.UseAuthorization();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseResponseCompression();

            app.UseMvc(); // Swagger

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages(); // Razor
            });
        }

        private static void SetupUserIdentity(IServiceCollection services)
        {
            services.AddIdentity<User, UserRole>((options) =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<CriThinkDbContext>()
                .AddDefaultTokenProviders();


            services.Configure<IdentityOptions>(options =>
            {
                // Password
                options.Password.RequiredLength = 8;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

                // User settings.
                options.User.RequireUniqueEmail = true;
            });
        }

        private void SetupJwtAuthentication(IServiceCollection services)
        {
            var audience = Configuration["Jwt-Audience"];
            var issuer = Configuration["Jwt-Issuer"];
            var key = Configuration["Jwt-SecretKey"];
            var keyBytes = Encoding.ASCII.GetBytes(key);

            services.AddAuthorization();

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidIssuer = issuer
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            // TODO: log
                            Debug.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            // TODO: log
                            Debug.WriteLine("OnTokenValidated: " + context.SecurityToken);
                            return Task.CompletedTask;
                        }
                    };
                });
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
            if (!_environment.IsDevelopment())
                return;

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter into field the word 'Bearer' following by space and the JWT",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                options.OperationFilter<AddRequiredHeaderParameter>();

                var contact = new OpenApiContact
                {
                    Name = Configuration["SwaggerApiInfo:name"],
                    Email = Configuration["SwaggerApiInfo:email"],
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

        private void SetupSettings(IServiceCollection services)
        {
            services.Configure<AwsSESSettings>(Configuration.GetSection(nameof(AwsSESSettings)));
        }

        private void SetupInternalServices(IServiceCollection services)
        {
            // Email Sender
            services.AddEmailSenderService();

            // Identity
            services.AddTransient<IIdentityService, IdentityService>();

            // NewsAnalyzer
            var azureEndpoint = Configuration["Azure-Cognitive-Endpoint"];
            var azureCredentials = Configuration["Azure-Cognitive-KeyCredentials"];
            services.AddNewsAnalyzerProvider(azureCredentials, azureEndpoint);
            services.AddTransient<INewsAnalyzerService, NewsAnalyzerService>();

            // DomainAnalyzer
            services.AddDomainAnalyzerProvider();
            services.AddTransient<IDomainAnalyzerFacade, DomainAnalyzerFacade>();

            // NewsSource
            services.AddTransient<INewsAnalyzerFacade, NewsAnalyzerFacade>();
            services.AddTransient<INewsSourceService, NewsSourceService>();

            // Infrastructure
            services.AddTransient<INewsSourceRepository, NewsSourceRepository>();

            var redisConnectionString = Configuration.GetConnectionString("CriThinkRedisCacheConnection");
            services.AddInfrastructure(redisConnectionString);
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
