using System.Threading.Tasks;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Core.Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DfT.ZEV.Administration.Web.Controllers;

[Route("data")]
public class DataController : Controller
{
    private readonly IOptions<BucketsConfiguration> _bucketOptions;
    private readonly IStorageService _storageService;

    public DataController(IOptions<BucketsConfiguration> bucketOptions, IStorageService storageService)
    {
        _bucketOptions = bucketOptions;
        _storageService = storageService;
    }

    [HttpGet("upload")]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet("upload-success")]
    public IActionResult UploadSuccess()
    {
        return View();
    }
    
    [HttpPost("upload-file")]
    public async Task<IActionResult> UploadFile()
    {
        var file = Request.Form.Files[0];
        if (file.Length > 0)
        {
            await using var fileStream = file.OpenReadStream();

            var dto = new UploadFileToBucketDto
            {
                Filename = file.FileName,
                Bucket = _bucketOptions.Value.ManufacturerImport,
                FileStream = file.OpenReadStream()
            };
            await _storageService.UploadFile(dto);
        }
        return Ok(new {Message = "File uploaded successfully!"});
    }
}