using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    public class TokenService : ServiceCommonOneToMany<Token, IToken>, ISearchableServiceCommon<Token,IToken>
    {
        public Expression<Func<Token, string>> PropertyExpression => e => e.Text;

        public override string RelatedEntity => "User";

        public IToken Get(string tokenText)
        {
            return Repo.Get(tokenText, PropertyExpression);
        }

        public List<IToken> Search(string tokenText)
        {
            return Repo.Search(tokenText, PropertyExpression);
        }
    }
}