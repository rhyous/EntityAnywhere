using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Rhyous.EntityAnywhere.WebApi
{
    public interface IEntityControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
    }
}