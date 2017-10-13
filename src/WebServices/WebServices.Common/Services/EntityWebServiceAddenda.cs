using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    public class EntityWebServiceAddenda<TEntity> : IEntityWebServiceAddenda
    {
        public virtual List<Addendum> GetAddenda(string id)
        {
            var entityName = typeof(TEntity).Name;
            return AddendaService.Get(x => x.Entity == entityName && x.EntityId == id.ToString())
                                 .ToConcrete<Addendum>().ToList();
        }

        public virtual List<Addendum> GetAddendaByEntityIds(List<string> entityIds)
        {
            var entityName = typeof(TEntity).Name;
            return AddendaService.Get(x => x.Entity == entityName && entityIds.Contains(x.EntityId))
                                 .ToConcrete<Addendum>().ToList();
        }

        /// <summary>
        /// Gets and addendum value for the entity id by the addendum name.
        /// </summary>
        /// <param name="id">The Entity Id.</param>
        /// <param name="name">The property name of the addendum.</param>
        /// <returns>The value of the addendum.</returns>
        public virtual Addendum GetAddendaByName(string id, string name)
        {
            var entityName = typeof(TEntity).Name;
            return AddendaService.Get(x => x.Entity == entityName && x.EntityId == id.ToString() && x.Property.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                                 .OrderByDescending(x => x.CreateDate)
                                 .FirstOrDefault()
                                 .ToConcrete<Addendum>();
        }


        #region Injectable Dependency
        protected virtual IServiceCommon<Addendum, IAddendum, long> AddendaService
        {
            get { return _AddendaService ?? (_AddendaService = new ServiceCommon<Addendum, IAddendum, long>()); }
            set { _AddendaService = value; }
        } private IServiceCommon<Addendum, IAddendum, long> _AddendaService;        
        #endregion

    }
}
