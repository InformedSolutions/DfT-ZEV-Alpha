namespace DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;

public class ManufacturerUsersDto
{
    public Guid Id { get; set; }
}

public class GetManufacturerByIdQueryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    
    public IEnumerable<ManufacturerUsersDto> Users { get; set; } = null!;
    
}