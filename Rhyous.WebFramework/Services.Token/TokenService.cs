using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public class TokenService : ServiceCommonOneToMany<Token, IToken>
    {
        public override string RelatedEntity => "User";
    }
}