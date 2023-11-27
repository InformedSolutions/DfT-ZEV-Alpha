using DfT.ZEV.Common.Attributes;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DfT.ZEV.Administration.Web.Extensions.Conventions;

public class RemoveActionConvention : IApplicationModelConvention
{
    private readonly bool _isUserPasswordManagementEnabled;
    private readonly bool _isUserEmailChangeEnabled;
    private readonly bool _isUserRegistrationEnabled;

    public RemoveActionConvention(IConfiguration configuration)
    {
        _isUserPasswordManagementEnabled = configuration.GetValue<bool>("UserPasswordManagementEnabled");
        _isUserEmailChangeEnabled = configuration.GetValue<bool>("UserEmailChangeEnabled");
        _isUserRegistrationEnabled = configuration.GetValue<bool>("UserRegistrationEnabled");
    }

    public void Apply(ApplicationModel application)
    {
        if (_isUserRegistrationEnabled &&
            _isUserPasswordManagementEnabled &&
            _isUserEmailChangeEnabled)
        {
            return;
        }

        foreach (var controller in application.Controllers)
        {
            var toBeRemoved = new List<ActionModel>();
            foreach (var action in controller.Actions)
            {
                if (ShouldBeRemoved(action))
                {
                    toBeRemoved.Add(action);
                }
            }

            foreach (var action in toBeRemoved)
            {
                controller.Actions.Remove(action);
            }
        }
    }

    private bool ShouldBeRemoved(ActionModel action)
    {
        return false;
    }
}