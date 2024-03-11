using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public interface IBearerDecoder
    {
        Task<IAccessToken> DecodeAsync(string tokenText);
    }
}