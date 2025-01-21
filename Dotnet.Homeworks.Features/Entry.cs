using Dotnet.Homeworks.DataAccess;
using Dotnet.Homeworks.Features.Helpers;
using Dotnet.Homeworks.Infrastructure;
using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Features;

public static class Entry
{
    public static void AddFeatures(this IServiceCollection services)
    {
        services.AddMediator(typeof(Entry).Assembly);
        services.AddPipelineBehaviors(
            @namespace: UserManagement.DirectoryReference.Namespace,
            AssemblyReference.Assembly, 
            AssemblyReference.Assembly);
        services.AddDataAccess();
        services.AddInfrastructure();
    }
}