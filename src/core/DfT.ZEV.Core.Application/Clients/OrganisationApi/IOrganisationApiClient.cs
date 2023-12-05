using DfT.ZEV.Core.Application.Accounts.Commands.CreateUser;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;

namespace DfT.ZEV.Core.Application.Clients.OrganisationApi;

public interface IOrganisationApiClient
{
    Task<GetAllManufacturersQueryResponse?> GetManufacturersAsync(string search);
    Task<GetManufacturerByIdQueryDto?> GetManufacturerByIdAsync(Guid id);
    Task<CreateUserCommandResponse?> CreateUserAsync(CreateUserCommand request);
}