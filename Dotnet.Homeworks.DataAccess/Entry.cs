using Dotnet.Homeworks.DataAccess.Repositories;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.DataAccess;

public static class Entry
{
    public static void AddDataAccess(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
    }
}