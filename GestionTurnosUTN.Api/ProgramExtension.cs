using Dsw2025Tpi.Data.Repositories;
using GestionTurnosUTN.Application.Interfaces;
using GestionTurnosUTN.Application.Services;
using GestionTurnosUTN.Data;
using GestionTurnosUTN.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestionTurnosUTN.Api;

public static class ProgramExtension
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRepository, EfRepository>();
        services.AddDbContext<GestionTurnosUTNDomainContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("GestionTurnosUTNEntities"))
        );
        return services;
    }
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ITurnManagementService, TurnManagementService>();
        // Aquí puedes agregar tus servicios de dominio, por ejemplo:
        // services.AddScoped<IMiServicioDeDominio, MiServicioDeDominio>();

        return services;
    }
}
