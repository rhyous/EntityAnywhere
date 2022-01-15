using System.Reflection;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public static class MethodInfoExtensions
    {
        public static async Task<T> InvokeAsync<T>(this MethodInfo mi, object obj, params object[] parameters)
        {
            var task = mi.Invoke(obj, parameters) as Task<T>;
            return await task.ConfigureAwait(false);            
        }
    }
}