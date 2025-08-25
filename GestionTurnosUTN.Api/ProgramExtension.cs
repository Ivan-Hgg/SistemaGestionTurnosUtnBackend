using Dsw2025Tpi.Data.Repositories;
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
}
