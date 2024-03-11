using Microsoft.Extensions.DependencyInjection;

namespace Rhyous.EntityAnywhere.Security
{
    public static class TokenAuthenticationExtensions 
    {
        public static void UseTokenAuthentication(this IServiceCollection services) 
        {
            services.AddAuthentication(options => {
                options.DefaultScheme = TokenAuthenticationSchemeOptions.Name;
            })
            .AddScheme<TokenAuthenticationSchemeOptions, TokenAuthenticationHandler>(
                TokenAuthenticationSchemeOptions.Name, option => {}
            );
        }
    } 
}