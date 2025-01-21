using Dotnet.Homeworks.Features.Helpers;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;
using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;
using FluentValidation;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.Infrastructure;

public static class Infrastracture
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
        services.AddPermissionChecks(new[] {AssemblyReference.Assembly});
        services.AddMediator();
        services.AddPipelineBehaviors(Features.UserManagement.DirectoryReference.Namespace,
            Helpers.AssemblyReference.Assembly, 
            AssemblyReference.Assembly);

        return services;
    }
}