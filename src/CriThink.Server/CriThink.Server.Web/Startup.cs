using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CriThink.Common.Endpoints;
using CriThink.Common.Endpoints.Converters;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Application;
using CriThink.Server.Core.Delegates;
using CriThink.Server.Core.Entities;
using CriThink.Server.Infrastructure;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.SocialProviders;
using CriThink.Server.Providers.DebunkingNewsFetcher.Settings;
using CriThink.Server.Providers.EmailSender.Settings;
using CriThink.Server.Web.ActionFilters;
using CriThink.Server.Web.BackgroundServices;
using CriThink.Server.Web.HealthCheckers;
using CriThink.Server.Web.Middlewares;
using CriThink.Server.Web.Services;
using CriThink.Server.Web.Swagger;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
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
            SetupKestrelOptions(services);

            SetupPostgreSqlConnection(services);

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

            SetupLocalization(services);

            SetupControllers(services);

            SetupExternalLoginProviders(services);

            SetupHealthChecks(services);

            SetupBackgroundServices(services);
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            var opt = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(opt);
#pragma warning restore CA1062 // Validate arguments of public methods

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseLiveReload(); // LiveReload

                app.UseSwagger(); // Swagger
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    options.InjectStylesheet("/swagger-custom/swaggerstyle.css");
                    options.InjectJavascript("/swagger-custom/swaggerstyle.js");
                    options.DisplayRequestDuration();
                });
            }

            app.UseResponseCaching();

            app.UseApiVersioning();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={604800}");
                },
            });

            app.UseRouting();

            app.UseCors(AllowSpecificOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseResponseCompression();

#pragma warning disable MVC1005 // Cannot use UseMvc with Endpoint Routing.
            app.UseMvc() // It doesn't detect the setting because it's outside ConfigureServices
                .UseStatusCodePagesWithRedirects("/error/{0}");
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

        private void SetupKestrelOptions(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));
        }

        private void SetupPostgreSqlConnection(IServiceCollection services)
        {
            services.AddDbContext<CriThinkDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("CriThinkDbPgSqlConnection");
                options.UseNpgsql(connectionString, npgsqlOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(15),
                        errorCodesToAdd: null);
                })
                .UseSnakeCaseNamingConvention(System.Globalization.CultureInfo.InvariantCulture);
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
            var keyBytes = Encoding.UTF8.GetBytes(key);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/backoffice/account/";
                    options.LogoutPath = "/backoffice/account/logout";
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
                        ValidIssuer = issuer,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,
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
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please insert the Bearer token",
                        Name = HeaderNames.Authorization,
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
                                Id = JwtBearerDefaults.AuthenticationScheme,
                            },
                        },
                        Array.Empty<string>()
                    }
                });

                options.OperationFilter<AddRequiredHeaderParameter>();
                options.OperationFilter<SwaggerLanguageHeader>();

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
            services.Configure<OpenOnlineSettings>(Configuration.GetSection("DebunkingNewsProviders:OpenOnline"));
            services.Configure<Channel4Settings>(Configuration.GetSection("DebunkingNewsProviders:Channel4"));
            services.Configure<FullFactSettings>(Configuration.GetSection("DebunkingNewsProviders:FullFact"));
            services.Configure<FactaNewsSettings>(Configuration.GetSection("DebunkingNewsProviders:FactaNews"));
        }

        private static void SetupMediatR(IServiceCollection services)
        {
            services.AddMediatR(typeof(Infrastructure.Bootstrapper), typeof(Application.Bootstrapper));
            services.AddFluentValidation(new[] { typeof(Application.Bootstrapper).Assembly });
        }

        private static void SetupInternalServices(IServiceCollection services)
        {
            // Core
            Core.Bootstrapper.AddCore(services);

            // Infrastructure
            services.AddInfrastructure();

            // Application
            services.AddApplication();

            // Services
            services.AddSingleton<IAppVersionService, AppVersionService>();
        }

        private static void SetupErrorHandling(IServiceCollection services)
        {
            services.AddScoped<ScraperAuthenticationFilter>();
            services.AddScoped<DebunkingNewsTriggerAuthenticationFilter>();
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
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            if (_environment.IsDevelopment())
                mvcBuilder.AddRazorRuntimeCompilation();
        }

        private static void SetupAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(Application.Bootstrapper));
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

        private static void SetupLocalization(IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new()
                {
                    new CultureInfo("en"),
                    new CultureInfo("it")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.ApplyCurrentCultureToResponseHeaders = true;
            });
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
                .AddCheck<PostgreSqlHealthChecker>(EndpointConstants.HealthCheckPostgreSql, HealthStatus.Unhealthy, tags: new[] { EndpointConstants.HealthCheckPostgreSql })
                .AddDbContextCheck<CriThinkDbContext>(EndpointConstants.HealthCheckDbContext, HealthStatus.Unhealthy, tags: new[] { EndpointConstants.HealthCheckDbContext });
        }

        private static void SetupBackgroundServices(IServiceCollection services)
        {
            services.AddHostedService<RefreshTokenCleanerBackgroundService>();
            services.AddHostedService<UserPendingDeletionCleanerBackgroundService>();

            services.Configure<HostOptions>(
                opts => opts.ShutdownTimeout = TimeSpan.FromMinutes(1));
        }

        private static void MapHealthChecks(IEndpointRouteBuilder endpoints)
        {
            // Redis
            endpoints.MapHealthChecks(
                GetServiceHealthPath(EndpointConstants.HealthCheckRedis),
                GetHealthCheckFilter(EndpointConstants.HealthCheckRedis));

            // PostgreSQL
            endpoints.MapHealthChecks(
                GetServiceHealthPath(EndpointConstants.HealthCheckPostgreSql),
                GetHealthCheckFilter(EndpointConstants.HealthCheckPostgreSql));

            // PostgreSQL DbContext
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

        private static void SetupExternalLoginProviders(IServiceCollection services)
        {
            services.AddTransient<FacebookProvider>();
            services.AddTransient<GoogleProvider>();
            services.AddTransient<AppleProvider>();

            services.AddTransient<ExternalLoginProviderResolver>(serviceProvider => externalProvider =>
            {
                return externalProvider switch
                {
                    ExternalLoginProvider.Facebook => serviceProvider.GetService<FacebookProvider>(),
                    ExternalLoginProvider.Google => serviceProvider.GetService<GoogleProvider>(),
                    ExternalLoginProvider.Apple => serviceProvider.GetService<AppleProvider>(),
                    _ => throw new NotSupportedException()
                };
            });
        }
    }
}
