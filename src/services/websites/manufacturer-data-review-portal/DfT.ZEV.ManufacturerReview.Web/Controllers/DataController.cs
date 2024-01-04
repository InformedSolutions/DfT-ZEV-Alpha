using System.Threading;
using System.Threading.Tasks;
using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Core.Application;
using DfT.ZEV.Core.Infrastructure.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DfT.ZEV.ManufacturerReview.Web.Controllers;

public class DataController : Controller
{
    private readonly IOptions<BucketsConfiguration> _bucketOptions;
    private readonly IStorageService _storageService;

    public DataController(IOptions<BucketsConfiguration> bucketOptions, IStorageService storageService)
    {
        _bucketOptions = bucketOptions;
        _storageService = storageService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
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
        return View();
    }
}