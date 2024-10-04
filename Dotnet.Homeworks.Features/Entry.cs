using Dotnet.Homeworks.DataAccess;
using Dotnet.Homeworks.Features.Helpers;
using Dotnet.Homeworks.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Features;

public static class Entry
{
    public static void AddFeatures(this IServiceCollection services)
    {
        services.AddMediatR(conf => conf.RegisterServicesFromAssembly(AssemblyReference.Assembly));
        services.AddDataAccess();
        services.AddInfrastructure();
    }
}