using MediatR;

namespace DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;

public class GetManufacturerByIdQuery : IRequest<GetManufacturerByIdQueryDto>
{
    public Guid Id { get; set; }
    
    public GetManufacturerByIdQuery(Guid id)
    {
        Id = id;
    }
}