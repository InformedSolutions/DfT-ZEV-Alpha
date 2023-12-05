using System;
using System.Collections.Generic;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;

namespace DfT.ZEV.Administration.Web.Models;

public class ManageOrganizationViewModel
{
    public GetManufacturerByIdQueryDto Dto { get; set; }
    public Guid Id { get; set; }
    
    public string UserEmail { get; set; } = null!;
    public IEnumerable<Guid> UserPermissionIds { get; set; } = null!;
}