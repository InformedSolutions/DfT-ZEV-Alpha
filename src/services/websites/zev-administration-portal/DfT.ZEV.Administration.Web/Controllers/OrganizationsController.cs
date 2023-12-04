using System.Linq;
using System.Threading.Tasks;
using DfT.ZEV.Administration.Web.Models;
using DfT.ZEV.Core.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DfT.ZEV.Administration.Web.Controllers;

[Route("organization")]
public class OrganizationsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    
    public OrganizationsController(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
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
}