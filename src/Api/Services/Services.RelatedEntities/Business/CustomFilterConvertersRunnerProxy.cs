using Autofac;
using Rhyous.Odata.Filter;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    public class CustomFilterConvertersRunnerProxy<TEntity> : ICustomFilterConvertersRunner<TEntity>
    {
        private readonly ILifetimeScope _Scope;

        public CustomFilterConvertersRunnerProxy(ILifetimeScope scope)
        {
            _Scope = scope;
        }

        public async Task<Filter<TEntity>> ConvertAsync(Filter<TEntity> filter)
        {
            using (var proxyscope = _Scope.BeginLifetimeScope(b =>
                {
                    b.RegisterModule<FilterConverterRegistrationModule>();
                }))
            {
                var runner = proxyscope.Resolve<ICustomFilterConvertersRunner<TEntity>>();
                return await runner.ConvertAsync(filter);
            }            
        }
    }
}
