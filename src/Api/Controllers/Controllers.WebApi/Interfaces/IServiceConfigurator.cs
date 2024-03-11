
using Microsoft.Extensions.DependencyInjection;

namespace Rhyous.EntityAnywhere.WebApi
{
    public interface IServiceConfigurator
    {
        void Configure(IServiceCollection services);
    }
}