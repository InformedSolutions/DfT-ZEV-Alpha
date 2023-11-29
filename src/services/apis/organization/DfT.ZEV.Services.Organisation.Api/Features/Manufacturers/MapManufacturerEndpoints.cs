using DfT.ZEV.Core.Application.Manufacturers.Commands.CreateManufacturer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Services.Organisation.Api.Features.Manufacturers;

public static class MapManufacturerEndpointsExtensions
{
    public static void MapManufacturerEndpoints(this WebApplication app)
    {
        // app.MapGet("/manufacturers/", GetAllManufacturersHandler.HandleAsync)
        //     .WithTags("Manufacturers");

        app.MapPost("/manufacturers/", CreateManufacturer)
            .WithTags("Manufacturers");

        // app.MapGet("/manufacturers/{id}", GetManufacturerByIdHandler.HandleAsync)
        //     .WithTags("Manufacturers");
        //
        // app.MapPut("/manufacturers/{id}", UpdateManufacturerHandler.HandleAsync)
        //     .WithTags("Manufacturers");
        //
        // app.MapDelete("/manufacturers/{id}", DeleteManufacturerHandler.HandleAsync)
        //     .WithTags("Manufacturers");
    }
    
    private static async Task<IResult> CreateManufacturer([FromBody] CreateManufacturerCommand request, [FromServices] IMediator mediator, 
        CancellationToken cancellationToken = default)
        => Results.Ok(await mediator.Send(request, cancellationToken));
}