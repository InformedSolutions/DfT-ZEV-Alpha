using idunno.Authentication.Basic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace DfT.ZEV.ManufacturerReview.Web.Extensions.Conventions;

public class BasicAuthConvention : IControllerModelConvention
{
    private readonly bool _isBasicAuthEnabled;

    public BasicAuthConvention(IConfiguration configuration)
    {
        _isBasicAuthEnabled = configuration.GetSection("BasicAuth").GetValue<bool>("IsEnabled");
    }

    public void Apply(ControllerModel controller)
    {
        if (!_isBasicAuthEnabled)
        {
            // Basic auth is disabled, do nothing
            return;
        }

        // Basic auth enabled. Add Basic Auth Attribute to all not authorized actions.
        var isControllerAuthorized = controller.Attributes.Any(x => x is AuthorizeAttribute);

        foreach (var action in controller.Actions)
        {
            var isActionForcedAnonymous = action.Attributes.Any(x => x is AllowAnonymousAttribute);
            if (isActionForcedAnonymous)
            {
                // We want actions with AllowAnonymousAttribute to be open on basic auth
                continue;
            }

            var isActionAuthorized = action.Attributes.Any(x => x is AuthorizeAttribute);

            if (!(isControllerAuthorized || isActionAuthorized))
            {
                // Action is not authorized in any way. Add basic auth to action
                action.Filters.Add(new AuthorizeFilter(new IAuthorizeData[]
                {
                    new AuthorizeAttribute
                    {
                        AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme,
                    },
                }));
            }
        }
    }
}
