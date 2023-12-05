using System.Collections.Generic;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetAllManufacturers;


namespace DfT.ZEV.Administration.Web.Models;

public class OrganisationsViewModel
{
    public IEnumerable<GetAllManufacturersDto> Manufacturers { get; set; }
    public string SearchTerm { get; set; } 
}