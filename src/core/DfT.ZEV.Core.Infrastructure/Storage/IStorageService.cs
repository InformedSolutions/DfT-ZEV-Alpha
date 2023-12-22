
namespace DfT.ZEV.Core.Infrastructure.Storage;

public interface IStorageService
{
    Task UploadFile(UploadFileToBucketDto dto, CancellationToken ct = default);
}