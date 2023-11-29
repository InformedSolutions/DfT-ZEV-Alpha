using DfT.ZEV.Core.Application.Manufacturers.Commands.CreateManufacturer;
using DfT.ZEV.Core.Application.Manufacturers.Commands.DeleteManufacturer;
using DfT.ZEV.Core.Application.Manufacturers.Commands.UpdateManufacturer;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Services.Organisation.Api.Features.Manufacturers;

public static class MapManufacturerEndpointsExtensions
{
    public static void MapManufacturerEndpoints(this WebApplication app)
    {
        app.MapGet("/manufacturers/{id}", GetManufacturerById)
            .WithTags("Manufacturers");
        
        app.MapGet("/manufacturers/", GetAllManufacturers)
            .WithTags("Manufacturers");

        app.MapPost("/manufacturers/", CreateManufacturer)
            .WithTags("Manufacturers");
        
        app.MapPut("/manufacturers/{id}", UpdateManufacturer)
            .WithTags("Manufacturers");
        
        app.MapDelete("/manufacturers/{id}", DeleteManufacturer)
             .WithTags("Manufacturers");
    }
    
    private static async Task<IResult> CreateManufacturer([FromBody] CreateManufacturerCommand request, [FromServices] IMediator mediator, 
        CancellationToken cancellationToken = default)
        => Results.Ok(await mediator.Send(request, cancellationToken));
    
    private static async Task<IResult> GetAllManufacturers([FromServices] IMediator mediator, CancellationToken ct)
        => Results.Ok(await mediator.Send(new GetAllManufacturersQuery(), ct));
    
    private static async Task<IResult> GetManufacturerById([FromRoute] Guid id,[FromServices] IMediator mediator, CancellationToken ct)
        => Results.Ok(await mediator.Send(new GetManufacturerByIdQuery(id), ct));
    
    private static async Task<IResult> DeleteManufacturer([FromRoute] Guid id,[FromServices] IMediator mediator, CancellationToken ct)
        => Results.Ok(await mediator.Send(new DeleteManufacturerCommand(id), ct));
    
    private static async Task<IResult> UpdateManufacturer([FromRoute] Guid id,[FromBody] UpdateManufacturerData data,[FromServices] IMediator mediator, CancellationToken ct)
        => Results.Ok(await mediator.Send(new UpdateManufacturerCommand(id,data), ct));
}