using System.Reflection;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    // public static void AddPermissionChecks(
    //     this IServiceCollection serviceCollection,
    //     Assembly assembly
    // )
    // {
    //     throw new NotImplementedException();
    // }
    
    public static void AddPermissionChecks(
        this IServiceCollection serviceCollection,
        Assembly[] assemblies
    )
    {
        serviceCollection.AddScoped<IPermissionCheck, PermissionCheck>();
        serviceCollection.AddHttpContextAccessor();
        var tuples = PermissionCheck.GetPermissionChecksFrom(assemblies);
        tuples.ForEach(tuple =>
        {
            serviceCollection.AddScoped(tuple.Iface, tuple.Impl);
        });

        serviceCollection.AddScoped<IPermissionCheck, PermissionCheck>();
    }
}