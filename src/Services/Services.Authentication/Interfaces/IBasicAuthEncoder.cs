using Rhyous.EntityAnywhere.Interfaces;
using System.Text;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IBasicAuthEncoder
    {
        Credentials Decode(string encodedHeader, Encoding encoding);
    }
}