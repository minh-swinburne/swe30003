using Humanizer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SmartRide.WebAPI.Controllers.Conventions;

public class KebabCaseControllerConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        foreach (var selector in controller.Selectors)
        {
            if (selector.AttributeRouteModel != null)
            {
                // Convert the route template to kebab-case
                selector.AttributeRouteModel.Template = selector.AttributeRouteModel?.Template?
                    .Replace("[controller]", controller.ControllerName.Kebaberize());
            }
        }
    }
}