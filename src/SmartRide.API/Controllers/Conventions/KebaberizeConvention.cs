using Humanizer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SmartRide.API.Controllers.Conventions;

public class KebaberizeConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        foreach (var selector in controller.Selectors)
        {
            if (selector.AttributeRouteModel != null)
            {
                // Convert the controller name to kebab-case
                controller.ControllerName = controller.ControllerName.Kebaberize();
            }
        }
    }
}