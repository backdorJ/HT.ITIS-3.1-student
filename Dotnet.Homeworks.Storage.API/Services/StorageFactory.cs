using Dotnet.Homeworks.Storage.API.Dto.Internal;
using Minio;

namespace Dotnet.Homeworks.Storage.API.Services;

public class StorageFactory : IStorageFactory
{
    private readonly IMinioClient _client;

    public StorageFactory(IMinioClient client)
    {
        _client = client;
    }

    public async Task<IStorage<Image>> CreateImageStorageWithinBucketAsync(string bucketName)
    {
        await MakeBucketIfNotExistsAsync(bucketName);

        return new ImageStorage(_client, bucketName);
    }

    private async Task MakeBucketIfNotExistsAsync(string bucket, CancellationToken cancellationToken = default)
    {
        var isBucketExists = await _client.BucketExistsAsync(new BucketExistsArgs()
            .WithBucket(bucket), cancellationToken);

        if (isBucketExists)
        {
            return;
        }

        await _client.MakeBucketAsync(new MakeBucketArgs()
            .WithBucket(bucket), cancellationToken);
    }
}