using Microsoft.EntityFrameworkCore;
using Model;
using Model.Data;

namespace Service;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrm(this IServiceCollection services, IConfiguration configuration)
    {
        var ormType = configuration.GetValue<string>("ORMType");
        switch (ormType)
        {
            case "Ef":
                services.AddEf(configuration);
                break;
            case "Dapper":
                services.AddDapper(configuration);
                break;
            default:
                throw new Exception("Unknown ORMType");
        }

        return services;
    }
    
    private static IServiceCollection AddEf(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<DatabaseContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("AmusementParkDB"),
                cfg => cfg.MigrationsAssembly("Model")));

        services.AddEfData();
        
        return services;
    }

    private static IServiceCollection AddDapper(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory, NpgsqlDbConnectionFactory>(_ =>
            new NpgsqlDbConnectionFactory(configuration.GetConnectionString("AmusementParkDB")!));

        services.AddDapperData();
        
        return services;
    }
}