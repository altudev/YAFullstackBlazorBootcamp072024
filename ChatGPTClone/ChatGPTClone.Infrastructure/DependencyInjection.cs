﻿using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Domain.Settings;
using ChatGPTClone.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatGPTClone.Infrastructure
{
    // Bu sınıf, uygulama altyapısının bağımlılık enjeksiyonunu yapılandırır
    public static class DependencyInjection
    {
        // Bu metod, altyapı servislerini IServiceCollection'a ekler
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Veritabanı bağlantı dizesini yapılandırmadan alır
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // ApplicationDbContext'i PostgreSQL ile kullanmak üzere yapılandırır
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(connectionString));

            // IApplicationDbContext'i ApplicationDbContext ile eşler
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            // JWT ayarlarını yapılandırır
            ConfigureJwtSettings(services, configuration);

            return services;
        }

        // JWT ayarlarını yapılandıran özel metod
        private static void ConfigureJwtSettings(IServiceCollection services, IConfiguration configuration)
        {
            // Yapılandırmadan JWT ayarları bölümünü alır
            var jwtSettingsSection = configuration.GetSection("JwtSettings");

            // Eğer JWT ayarları yapılandırmada mevcutsa, bu ayarları kullanır
            if (jwtSettingsSection.Exists())
            {
                services.Configure<JwtSettings>(jwtSettingsSection);
            }
            // Aksi takdirde, varsayılan değerlerle JWT ayarlarını yapılandırır
            else
            {
                services.Configure<JwtSettings>(options =>
                {
                    options.SecretKey = "default-secret-key-for-development-only";
                    options.AccessTokenExpiration = TimeSpan.FromMinutes(30);
                    options.RefreshTokenExpiration = TimeSpan.FromDays(7);
                    options.Issuer = "ChatGPTClone";
                    options.Audience = "ChatGPTClone";
                });
            }
        }
    }
}
