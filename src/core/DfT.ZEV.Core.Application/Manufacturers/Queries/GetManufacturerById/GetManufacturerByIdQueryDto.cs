namespace DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;

public class GetManufacturerByIdQueryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}