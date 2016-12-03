
namespace Rhyous.WebFramework.Interfaces
{
    public interface ITokenValidator
    {
        bool IsValid(string token);
        IToken Token { get; set; }
    }
}