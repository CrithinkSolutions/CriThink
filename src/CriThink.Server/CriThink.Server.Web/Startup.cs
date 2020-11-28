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
using CriThink.Common.Endpoints.Converters;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Core.Delegates;
using CriThink.Server.Core.Entities;
using CriThink.Server.Infrastructure;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.SocialProviders;
using CriThink.Server.Providers.DebunkNewsFetcher.Settings;
using CriThink.Server.Providers.EmailSender.Settings;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.Facades;
using CriThink.Server.Web.HealthCheckers;
using CriThink.Server.Web.Middlewares;
using CriThink.Server.Web.Services;
using CriThink.Server.Web.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Westwind.AspNetCore.LiveReload;

// ReSharper disable UnusedMember.Global
namespace CriThink.Server.Web
{
    public class Startup
    {
        private const string AllowSpecificOrigins = "AllowSpecificOrigins";

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
            SetupSqlServerConnection(services);

            SetupUserIdentity(services);

            SetupAuthentication(services);

            SetupCache(services);

            SetupAPIVersioning(services);

            SetupSwagger(services);

            SetupSettings(services);

            SetupMediatR(services);

            SetupInternalServices(services);

            SetupErrorHandling(services);

            SetupGZipCompression(services);

            SetupCorsOrigins(services);

            SetupJsonSerializer(services);

            SetupAutoMapper(services);

            SetupRazorAutoReload(services);

            SetupControllers(services);

            SetupExternalLoginProviders(services);

            SetupHealthChecks(services);
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Called by framework")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseLiveReload(); // LiveReload

                app.UseSwagger(); // Swagger
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    options.InjectStylesheet("/swagger-custom/swaggerstyle.css");
                    options.DisplayRequestDuration();
                });
            }

            app.UseResponseCaching();

            app.UseApiVersioning();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(AllowSpecificOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseResponseCompression();

#pragma warning disable MVC1005 // Cannot use UseMvc with Endpoint Routing.
            app.UseMvc(); // It doesn't detect the setting because it's outside ConfigureServices
#pragma warning restore MVC1005 // Cannot use UseMvc with Endpoint Routing.

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area=BackOffice}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                MapHealthChecks(endpoints);
            });
        }

        private void SetupSqlServerConnection(IServiceCollection services)
        {
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

        private void SetupAuthentication(IServiceCollection services)
        {
            // JWT
            var audience = Configuration["Jwt-Audience"];
            var issuer = Configuration["Jwt-Issuer"];
            var key = Configuration["Jwt-SecretKey"];
            var keyBytes = Encoding.ASCII.GetBytes(key);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/backoffice/account/";
                    options.LogoutPath = "/backOffice/account/logout";
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.SlidingExpiration = true;

                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = (context) =>
                        {
                            context.Response.Redirect("/backOffice/account" + context.Request.QueryString);
                            return Task.CompletedTask;
                        },
                    };
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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

            // JWT + MVC
            services.AddAuthorization();
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
                options.SwaggerDoc("v1", new OpenApiInfo { Title = $"{Configuration["SwaggerApiInfo:Title"]}", Version = "v1", Contact = contact });

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
            services.Configure<User>(Configuration.GetSection("ServiceUser"));
            services.Configure<UserRole>(Configuration.GetSection("AdminRole"));
            services.Configure<EmailSettings>(Configuration.GetSection(nameof(EmailSettings)));
            services.Configure<WebSiteSettings>(Configuration.GetSection(nameof(WebSiteSettings)));
        }

        private static void SetupMediatR(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup), typeof(Bootstrapper));
        }

        private static void SetupInternalServices(IServiceCollection services)
        {
            // Core
            Core.Bootstrapper.AddCore(services);

            // Infrastructure
            services.AddInfrastructure();

            // Services
            services.AddSingleton<IAppVersionService, AppVersionService>();

            // Facades
            services.AddTransient<IDebunkingNewsServiceFacade, DebunkingNewsServiceFacade>();
        }

        private static void SetupErrorHandling(IServiceCollection services)
        {
            services.AddScoped<ApiValidationFilterAttribute>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        private static void SetupGZipCompression(IServiceCollection services)
        {
            services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
        }

        private void SetupCorsOrigins(IServiceCollection services)
        {
            var corsOrigins = Configuration.GetSection("AllowCorsOrigin").Get<string[]>();
            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificOrigins,
                    builder =>
                    {
                        foreach (var corsOrigin in corsOrigins)
                        {
                            builder.WithOrigins(corsOrigin)
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        }
                    });
            });
        }

        private void SetupJsonSerializer(IServiceCollection services)
        {
            var mvcBuilder = services
                .AddMvc(options => { options.EnableEndpointRouting = false; })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new NewsSourceClassificationConverter());
                });

            if (_environment.IsDevelopment())
                mvcBuilder.AddRazorRuntimeCompilation();
        }

        private static void SetupAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Core.Bootstrapper));
        }

        private void SetupRazorAutoReload(IServiceCollection services)
        {
            var mvcBuilder = services.AddRazorPages();

            if (_environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation(); // Razor
                services.AddLiveReload(); // LiveReload
            }
        }

        private static void SetupControllers(IServiceCollection services)
        {
            services.AddControllersWithViews(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                config.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        private static void SetupHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<RedisHealthChecker>(EndpointConstants.HealthCheckRedis, HealthStatus.Unhealthy, tags: new[] { EndpointConstants.HealthCheckRedis })
                .AddCheck<SqlServerHealthChecker>(EndpointConstants.HealthCheckSqlServer, HealthStatus.Unhealthy, tags: new[] { EndpointConstants.HealthCheckSqlServer })
                .AddDbContextCheck<CriThinkDbContext>(EndpointConstants.HealthCheckDbContext, HealthStatus.Unhealthy, tags: new[] { EndpointConstants.HealthCheckDbContext });
        }

        private static void MapHealthChecks(IEndpointRouteBuilder endpoints)
        {
            // Redis
            endpoints.MapHealthChecks(
                GetServiceHealthPath(EndpointConstants.HealthCheckRedis),
                GetHealthCheckFilter(EndpointConstants.HealthCheckRedis));

            // SQL Server
            endpoints.MapHealthChecks(
                GetServiceHealthPath(EndpointConstants.HealthCheckSqlServer),
                GetHealthCheckFilter(EndpointConstants.HealthCheckSqlServer));

            // SQL Server DbContext
            endpoints.MapHealthChecks(
                GetServiceHealthPath(EndpointConstants.HealthCheckDbContext),
                GetHealthCheckFilter(EndpointConstants.HealthCheckDbContext));
        }

        private static string GetServiceHealthPath(string serviceName) => $"{EndpointConstants.HealthCheckBase}{serviceName}";

        private static HealthCheckOptions GetHealthCheckFilter(string tag) => new HealthCheckOptions
        {
            Predicate = (check) => check.Tags.Contains(tag),
            AllowCachingResponses = false,
        };

        private void SetupExternalLoginProviders(IServiceCollection services)
        {
            var baseFacebookURL = Configuration["FacebookApiUrl"];

            services.AddHttpClient("Facebook", httpClient =>
            {
                httpClient.BaseAddress = new Uri(baseFacebookURL);
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
            }));

            var baseGoogleURL = Configuration["GoogleApiUrl"];

            services.AddHttpClient("Google", httpClient =>
            {
                httpClient.BaseAddress = new Uri(baseGoogleURL);
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
            }));

            var baseAppleURL = Configuration["AppleApiUrl"];

            services.AddHttpClient("Apple", httpClient =>
            {
                httpClient.BaseAddress = new Uri(baseAppleURL);
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
            }));

            services.AddTransient<FacebookProvider>();
            services.AddTransient<GoogleProvider>();
            services.AddTransient<AppleProvider>();

            services.AddTransient<ExternalLoginProviderResolver>(serviceProvider => externalProvider =>
            {
                switch (externalProvider)
                {
                    case ExternalLoginProvider.Facebook:
                        return serviceProvider.GetService<FacebookProvider>();
                    case ExternalLoginProvider.Google:
                        return serviceProvider.GetService<GoogleProvider>();
                    case ExternalLoginProvider.Apple:
                        return serviceProvider.GetService<AppleProvider>();
                    default:
                        throw new NotSupportedException();
                }
            });
        }
    }
}
