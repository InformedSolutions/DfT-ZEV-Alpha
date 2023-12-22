using System.Diagnostics;
using DfT.ZEV.Common.Configuration;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DfT.ZEV.Core.Infrastructure.Storage;

public class StorageService : IStorageService
{
    private readonly IOptions<BucketsConfiguration> _options;
    private readonly ILogger<StorageService> _logger;

    public StorageService(IOptions<BucketsConfiguration> options, ILogger<StorageService> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task UploadFile(UploadFileToBucketDto dto, CancellationToken ct = default)
    {
        //var fileSizeMb = Math.Round((double)file.Length / (1024 * 1024), 2); // Convert bytes to megabytes
        //var fileSizeGb = Math.Round(fileSizeMb / 1024, 2); // Convert megabytes to gigabytes
        _logger.LogInformation("Started uploading file to Google Cloud Storage: {FileName}", dto.Filename);
        var storageClient = await StorageClient.CreateAsync();
        
        // Upload the file to Google Cloud Storage
        var stopwatch = Stopwatch.StartNew();

        await storageClient.UploadObjectAsync(dto.Bucket, dto.Filename, null, dto.FileStream, cancellationToken: ct);
        stopwatch.Stop();

        _logger.LogInformation("Finished uploading file to Google Cloud Storage: {FileName} in {ElapsedMilliseconds}ms", dto.Filename, stopwatch.ElapsedMilliseconds);

    }
}