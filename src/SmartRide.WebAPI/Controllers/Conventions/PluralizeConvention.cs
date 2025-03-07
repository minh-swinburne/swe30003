using Humanizer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using SmartRide.WebAPI.Controllers.Attributes;

namespace SmartRide.WebAPI.Controllers.Conventions;

public class PluralizeConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        foreach (var selector in controller.Selectors)
        {
            if (selector.AttributeRouteModel != null && controller.Attributes.OfType<PluralizeAttribute>().Any())
            {
                // Pluralize the controller name
                controller.ControllerName = controller.ControllerName.Pluralize();
            }
        }
    }
}
