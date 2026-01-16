using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Model.Data;
using Model.Domain;
using Model.Domain.Validation;
using Model.Services;

namespace Model;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddModel(this IServiceCollection services)
    {
        services.AddServices();
        services.AddValidators();
        
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IVisitorService, VisitorService>();
        
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
        return services;
    }

    public static IServiceCollection AddEfData(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWorkEf>();
        
        return services;
    }
    
    public static IServiceCollection AddDapperData(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWorkDapper>();
        
        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<Visitor>, VisitorValidator>();
        
        return services;
    }
}