using MediatR;

namespace DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;

public class GetAllManufacturersQuery : IRequest<GetAllManufacturersQueryResponse>
{
    public string Search { get; }

    public GetAllManufacturersQuery(string search)
    {
        Search = search;
    }
 
}