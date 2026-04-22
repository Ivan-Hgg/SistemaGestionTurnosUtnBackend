using GestionTurnosUTN.Data.Identity;
using GestionTurnosUTN.Application.Interfaces;
using GestionTurnosUTN.Application.Services;
using GestionTurnosUTN.Data;
using GestionTurnosUTN.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GestionTurnosUTN.Data.Repositories;

namespace GestionTurnosUTN.Api;

public static class ProgramExtension
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRepository, EfRepository>();
        services.AddDbContext<GestionTurnosUTNDomainContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("GestionTurnosUTNEntities"))
        );
        services.AddDbContext<AuthenticateContext>(options =>
                   options.UseSqlServer(configuration.GetConnectionString("GestionTurnosUTNEntities"))
               ); 
        return services;
    }
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<INewsService, NewsManagementService>(); // Carlos: Agrego el servicio de gestión de noticias a la inyección de dependencias
        services.AddScoped<ITurnManagementService, TurnManagementService>();
        // Aquí puedes agregar tus servicios de dominio, por ejemplo:
        // services.AddScoped<IMiServicioDeDominio, MiServicioDeDominio>();

        return services;
    }
    public static IServiceCollection AddJWTServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        var jwtConfig = configuration.GetSection("Jwt");
        var keyText = jwtConfig["Key"] ?? throw new ArgumentNullException("JWT Key");
        var key = Encoding.UTF8.GetBytes(keyText);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig["Issuer"],
                ValidAudience = jwtConfig["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        services.AddSingleton<JwtTokenService>();

        return services;
    }
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddIdentity<IdentityUserExtension, IdentityRole>(options =>
        {
            options.Password = new PasswordOptions
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireNonAlphanumeric = true
            };

        })
        .AddEntityFrameworkStores<AuthenticateContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}
