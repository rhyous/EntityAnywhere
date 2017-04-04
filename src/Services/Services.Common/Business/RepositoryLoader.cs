using Rhyous.WebFramework.Interfaces;
using System.Linq;

namespace Rhyous.WebFramework.Services
{
    public class RepositoryLoader
    {
        public static IRepository<T, Tinterface, Tid> Load<T, Tinterface, Tid>()
            where T : class, Tinterface
            where Tinterface : IId<Tid>
        {
            return new EntityRepositoryLoader<T, Tinterface, Tid>().Plugins?.FirstOrDefault()
                ?? new EntityRepositoryLoaderCommon<T, Tinterface, Tid>().Plugins?.FirstOrDefault();
        }
    }
}
