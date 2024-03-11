using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityHandler<T>        
    {
        int Order { get; }
        Task<IEnumerable<T>> HandleAsync(IEnumerable<T> ts, object o = null);
    }
}