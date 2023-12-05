using System;
using System.Linq;
using System.Threading.Tasks;
using DfT.ZEV.Administration.Web.Models;
using DfT.ZEV.Core.Application.Accounts.Commands.CreateUser;
using DfT.ZEV.Core.Application.Clients.OrganisationApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DfT.ZEV.Administration.Web.Controllers;

[Authorize]
[Route("organisation")]
public class OrganisationsController : Controller
{
    private readonly IOrganisationApiClient _organisationApi;
    public OrganisationsController(IOrganisationApiClient organisationApi)
    {
        _organisationApi = organisationApi;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var res = await _organisationApi.GetManufacturersAsync("");
        var dto = new OrganisationsViewModel() { Manufacturers = res.Manufacturers };
        return this.View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Index(OrganisationsViewModel model)
    {
        model.SearchTerm = model.SearchTerm ?? string.Empty;
        var res = await _organisationApi.GetManufacturersAsync(model.SearchTerm);
        var dto = new OrganisationsViewModel() { Manufacturers = res.Manufacturers.ToList() };
        return this.View(dto);
    }

    [HttpGet("{id:guid}/manage")]
    public async Task<IActionResult> Manage(Guid id)
    {
        try
        {
            var manufacturer = await _organisationApi.GetManufacturerByIdAsync(id);
            var dto = new ManageOrganisationViewModel()
            {
                Id = id,
                Dto = manufacturer
            };
            return this.View(dto);

        }catch(Exception ex)
        {
            return this.NotFound();
        }
    }

    [HttpGet("{id:guid}/manage/add")]
    public async Task<IActionResult> ManageAddUser(Guid id)
    {
        return View();
    }

    [HttpPost("{id:guid}/manage/add")]
    public async Task<IActionResult> ManageAddUser(Guid id, ManageOrganisationAddUserModel model)
    {
        var command = new CreateUserCommand()
        {
            ManufacturerId = id,
            Email = model.Email,
            PermissionIds = new Guid[] { }
        };

        //var res = await _mediator.Send(command);
        var res = await _organisationApi.CreateUserAsync(command);
        return RedirectToAction("Manage", new { id = id });
    }

    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        var claims = User.Claims.ToList();
        if (claims.Any())
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