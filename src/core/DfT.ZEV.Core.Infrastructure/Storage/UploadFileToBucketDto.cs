namespace DfT.ZEV.Core.Infrastructure.Storage;

public class UploadFileToBucketDto
{
    public string Filename { get; set; }
    public string Bucket { get; set; }
    public Stream FileStream { get; set; }
}