using DfT.ZEV.Core.Application.Manufacturers.Commands.CreateManufacturer;
using DfT.ZEV.Core.Application.Manufacturers.Commands.DeleteManufacturer;
using DfT.ZEV.Core.Application.Manufacturers.Commands.UpdateManufacturer;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Services.Organisations.Api.Features.Manufacturers;

public static class MapManufacturerEndpointsExtension
{
    private const string ManufacturersPath = "/manufacturers/";
    private const string ManufacturerByIdPath = "/manufacturers/{id}";

    public static WebApplication MapManufacturerEndpoints(this WebApplication app)
    {
        app.MapGet(ManufacturersPath, GetAllManufacturers)
            .WithTags("Manufacturers");

        app.MapPost(ManufacturersPath, CreateManufacturer)
            .WithTags("Manufacturers");

        app.MapGet(ManufacturerByIdPath, GetManufacturerById);

        app.MapPut(ManufacturerByIdPath, UpdateManufacturer)
            .WithTags("Manufacturers");

        app.MapDelete(ManufacturerByIdPath, DeleteManufacturer)
             .WithTags("Manufacturers");

        return app;
    }

    private static async Task<IResult> CreateManufacturer([FromBody] CreateManufacturerCommand request, [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
        => Results.Ok(await mediator.Send(request, cancellationToken));

    private static async Task<IResult> GetAllManufacturers([FromServices] IMediator mediator, CancellationToken ct, [FromQuery] string search = "")
        => Results.Ok(await mediator.Send(new GetAllManufacturersQuery(search), ct));

    private static async Task<IResult> GetManufacturerById([FromRoute] Guid id, [FromServices] IMediator mediator, CancellationToken ct)
        => Results.Ok(await mediator.Send(new GetManufacturerByIdQuery(id), ct));

    private static async Task<IResult> DeleteManufacturer([FromRoute] Guid id, [FromServices] IMediator mediator, CancellationToken ct)
        => Results.Ok(await mediator.Send(new DeleteManufacturerCommand(id), ct));

    private static async Task<IResult> UpdateManufacturer([FromRoute] Guid id, [FromBody] UpdateManufacturerData data, [FromServices] IMediator mediator, CancellationToken ct)
        => Results.Ok(await mediator.Send(new UpdateManufacturerCommand(id, data), ct));
}