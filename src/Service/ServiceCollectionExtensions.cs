using Microsoft.EntityFrameworkCore;
using Model.Data;

namespace Service;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEf(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<DatabaseContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("AmusementParkDB"),
                cfg => cfg.MigrationsAssembly("Model")));
        
        return services;
    }

    public static IServiceCollection AddDapper(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory, NpgsqlDbConnectionFactory>(_ =>
            new NpgsqlDbConnectionFactory(configuration.GetConnectionString("AmusementParkDB")!));
        
        return services;
    }
}