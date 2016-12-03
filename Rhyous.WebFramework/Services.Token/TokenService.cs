using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public class TokenService : ServiceCommonOneToMany<Token, IToken>, ISearchableServiceCommon<Token,IToken>
    {
        public override string RelatedEntity => "User";

        public IToken Get(string tokenText)
        {
            return Repo.Get(tokenText, t => t.Text);
        }

        public List<IToken> Search(string tokenText)
        {
            return Repo.Search(tokenText, t => t.Text);
        }
    }
}