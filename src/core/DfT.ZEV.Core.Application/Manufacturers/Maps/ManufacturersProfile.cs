using AutoMapper;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;
using DfT.ZEV.Core.Domain.Manufacturers.Models;

namespace DfT.ZEV.Core.Application.Manufacturers.Maps;

public class ManufacturersProfile : Profile
{
    public ManufacturersProfile()
    {
        CreateMap<Manufacturer, GetAllManufacturersDto>();
    }
}