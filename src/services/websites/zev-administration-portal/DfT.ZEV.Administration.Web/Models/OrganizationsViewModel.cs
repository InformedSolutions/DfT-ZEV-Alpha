using System.Collections.Generic;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;
using DfT.ZEV.Core.Domain.Manufacturers.Models;

namespace DfT.ZEV.Administration.Web.Models;

public class OrganizationsViewModel
{
    public IEnumerable<GetAllManufacturersDto> Manufacturers { get; set; }
    public string SearchTerm { get; set; } 
}