using System;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;

namespace DfT.ZEV.Administration.Web.Models;

public class ManageOrganizationViewModel
{
    public GetManufacturerByIdQueryDto Dto { get; set; }
    public Guid Id { get; set; }
}