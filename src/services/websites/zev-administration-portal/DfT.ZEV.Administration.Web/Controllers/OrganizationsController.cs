using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DfT.ZEV.Administration.Web.Models;
using DfT.ZEV.Common.Services.Clients;
using DfT.ZEV.Core.Application.Accounts.Commands.CreateUser;
using DfT.ZEV.Core.Application.Manufacturers.Queries.GetManufacturerById;
using DfT.ZEV.Core.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Administration.Web.Controllers;

[Authorize]
[Route("organization")]
public class OrganizationsController : Controller
{
    private readonly OrganizationApiClient _organizationApi;
    public OrganizationsController(OrganizationApiClient organizationApi)
    {
      
        _organizationApi = organizationApi;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var res = await _organizationApi.GetManufacturersAsync("");
        var dto = new OrganizationsViewModel() { Manufacturers = res.Manufacturers };
        return this.View(dto);
    }
    
    [HttpPost]
    public async Task<IActionResult> Index(OrganizationsViewModel model)
    {
        model.SearchTerm = model.SearchTerm ?? string.Empty;
        var res = await _organizationApi.GetManufacturersAsync(model.SearchTerm);
        var dto = new OrganizationsViewModel() { Manufacturers = res.Manufacturers.ToList() };
        return this.View(dto);
    }
    
    [HttpGet("{id:guid}/manage")]
    public async Task<IActionResult> Manage(Guid id)
    {
        var manufacturer = await _organizationApi.GetManufacturerByIdAsync(id);
        if(manufacturer is null)
        {
            return this.NotFound();
        }
        
        var dto = new ManageOrganizationViewModel()
        {
            Id = id,
            Dto = manufacturer
        };
        return this.View(dto);
    }
    
    [HttpGet("{id:guid}/manage/add")]
    public async Task<IActionResult> ManageAddUser(Guid id)
    {
        return View();
    }

    [HttpPost("{id:guid}/manage/add")]
    public async Task<IActionResult> ManageAddUser(Guid id, ManageOrganizationAddUserModel model)
    {
        var command = new CreateUserCommand()
        {
            ManufacturerId = id,
            Email = model.Email,
            PermissionIds = new Guid[] {  }
        };

        //var res = await _mediator.Send(command);
        var res = await _organizationApi.CreateUserAsync(command);
        return RedirectToAction("Manage", new {id = id});
    }
    
    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        var claims = User.Claims.ToList();
        if(claims.Any())
        {
            return this.View(new TestViewModel()
            {
                IsAuthorized = true,
                Name = claims.FirstOrDefault(x => x.Type == "email")?.Value
            });
        }
        return this.View(new TestViewModel()
        {
            IsAuthorized = false
        });
    }
}