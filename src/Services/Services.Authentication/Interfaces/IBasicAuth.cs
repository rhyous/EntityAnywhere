using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IBasicAuth
    {
        Credentials Credentials { get; }
        string HeaderValue { get; }
    }
}