using Dotnet.Homeworks.Storage.API.Configuration;
using Minio;

namespace Dotnet.Homeworks.Storage.API.ServicesExtensions;

public static class AddMinioExtensions
{
    public static IServiceCollection AddMinioClient(this IServiceCollection services,
        MinioConfig minioConfiguration)
    {
        services.AddSingleton<IMinioClient, MinioClient>(_ => GetMinioClient(minioConfiguration));

        return services;
    }

    private static MinioClient GetMinioClient(MinioConfig conf)
    {
        return new MinioClient()
            .WithCredentials(conf.Username, conf.Password)
            .WithEndpoint(conf.Endpoint, conf.Port)
            .WithSSL(conf.WithSsl)
            .Build();
    }
}