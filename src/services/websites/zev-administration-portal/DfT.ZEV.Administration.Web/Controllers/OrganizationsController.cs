using System;
using System.Linq;
using System.Threading.Tasks;
using DfT.ZEV.Administration.Web.Models;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IAuthorizationService _authorizationService;
    public OrganizationsController(IMediator mediator, IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _authorizationService = authorizationService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var manufacturers = await  _unitOfWork.Manufacturers.GetAllAsync();
        var dto = new OrganizationsViewModel() { Manufacturers = manufacturers.OrderByDescending(x => x.UserBridges.Count).ToList() };
        return this.View(dto);
    }
    
    [HttpPost]
    public async Task<IActionResult> Index(OrganizationsViewModel model)
    {
        model.SearchTerm = model.SearchTerm ?? string.Empty;
        var manufacturers = await  _unitOfWork.Manufacturers.SearchAsync(model.SearchTerm);
        var dto = new OrganizationsViewModel() { Manufacturers = manufacturers.OrderByDescending(x => x.UserBridges.Count).ToList() };
        return this.View(dto);
    }
    
    [HttpGet("{id:guid}/manage")]
    public async Task<IActionResult> Manage(Guid id)
    {
        var manufacturer = await _mediator.Send(new GetManufacturerByIdQuery(id));
        if(manufacturer is null)
        {
            return this.NotFound();
        }
        
        //var users = await _unitOfWork.Users.GetUsersByManufacturerIdAsync(id);
        //var dto = new ManageOrganizationViewModel()
        //{
        //    Manufacturer = manufacturer,
        //    Users = users
        //};
        var dto = new ManageOrganizationViewModel()
        {
            Id = id,
            Dto = manufacturer
        };
        return this.View(dto);
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