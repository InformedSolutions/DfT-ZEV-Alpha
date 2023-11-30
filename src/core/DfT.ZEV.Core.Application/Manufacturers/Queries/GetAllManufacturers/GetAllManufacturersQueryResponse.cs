namespace DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;

public class GetAllManufacturersQueryResponse
{
    public IEnumerable<GetAllManufacturersDto> Manufacturers { get; set; } = new List<GetAllManufacturersDto>();
}