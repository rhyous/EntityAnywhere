using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    public interface ISearchableServiceCommon<T, Tinterface> : IServiceCommon<T,Tinterface>
        where T : class, Tinterface
    {
        Tinterface Get(string name);
        List<Tinterface> Search(string name);
        Expression<Func<T, string>> PropertyExpression { get; }
    }
}
