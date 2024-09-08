using System.Globalization;
using System.Text;
using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ChatGPTClone.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserService, CurrentUserManager>();

            services.AddSingleton<IEnvironmentService, EnvironmentManager>(sp => new EnvironmentManager(environment.WebRootPath));

            //Localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                // Set the default culture

                var defaultCulture = new CultureInfo("tr-TR");

                // Set supported cultures

                var supportedCultures = new List<CultureInfo>
                {
                    defaultCulture,
                    new CultureInfo("en-GB")
                };

                // Add supported cultures
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);

                options.SupportedCultures = supportedCultures;

                options.SupportedUICultures = supportedCultures;

                options.ApplyCurrentCultureToResponseHeaders = true;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var secretKey = configuration["JwtSettings:SecretKey"];

                if (string.IsNullOrEmpty(secretKey))
                    throw new ArgumentNullException("JwtSettings:SecretKey is not set.");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSwaggerGen(setupAction =>
            {

                setupAction.SwaggerDoc(
                    "v1",
                    new OpenApiInfo()
                    {
                        Title = "ChatGPTClone Web API",
                        Version = "1",
                        Description = "Through this API you can access ChatGPTClone App's details",
                        Contact = new OpenApiContact()
                        {
                            Email = "alper.tunga@yazilim.academy",
                            Name = "Alper Tunga",
                            Url = new Uri("https://yazilim.academy/")
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "© 2024 Yazılım Academy Tüm Hakları Saklıdır",
                            Url = new Uri("https://yazilim.academy/")
                        }
                    });

                setupAction.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = $"Input your Bearer token in this format - Bearer token to access this API",
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        }, new List<string>()
                    },
                });
            });

            return services;
        }
    }
}
