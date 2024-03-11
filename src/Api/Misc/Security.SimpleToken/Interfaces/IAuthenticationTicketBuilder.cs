using Microsoft.AspNetCore.Authentication;

namespace Rhyous.EntityAnywhere.Security
{
    public interface IAuthenticationTicketBuilder 
    {
        AuthenticationTicket BuildAdmin();
        AuthenticationTicket Build(string token);
    }
}