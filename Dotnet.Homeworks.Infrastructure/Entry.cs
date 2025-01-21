using System.Reflection;
using Dotnet.Homeworks.Infrastructure.Services;

namespace Dotnet.Homeworks.Infrastructure;

public static class Entry
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IRegistrationService, RegistrationService>();

        return services;
    }
}