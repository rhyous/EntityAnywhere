
using Microsoft.AspNetCore.Builder;

namespace Rhyous.EntityAnywhere.WebApi
{
    public interface IAppConfigurator
    {
        void Configure(WebApplication app);
    }
}