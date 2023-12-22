using DfT.ZEV.Common.Configuration;
using DfT.ZEV.Core.Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace DfT.ZEV.Services.SchemeData.Api.Features.Files;

public static class MapFilesEndpointsExtension
{
    private const string FilesPath = "/files/";

    public static WebApplication MapFilesEndpoints(this WebApplication app)
    {
        app.MapPost(FilesPath, UploadFile)
            .WithTags("Files")
            .Accepts<IFormFile>("multipart/form-data")
            ;

        return app;
    }

    private static async Task<IResult> UploadFile(HttpRequest request,[FromServices] IOptions<BucketsConfiguration> bucketOptions, [FromServices] IStorageService storageService, CancellationToken cancellationToken = default)
    {
        var file = request.Form.Files[0];

        if (file.Length > 0)
        {
            await using var fileStream = file.OpenReadStream();

            var dto = new UploadFileToBucketDto
            {
                Filename = file.FileName,
                Bucket = bucketOptions.Value.ManufacturerImport,
                FileStream = file.OpenReadStream()
            };
            await storageService.UploadFile(dto, cancellationToken);
        }
        return Results.Ok();
    }
}