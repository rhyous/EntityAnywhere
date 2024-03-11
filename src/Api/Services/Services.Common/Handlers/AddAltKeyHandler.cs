using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    class AddAltKeyHandler<TEntity, TInterface, TId, TAltKey> : IAddAltKeyHandler<TEntity, TInterface, TId, TAltKey>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
           where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        private readonly IAddHandler<TEntity, TInterface, TId> _AddHandler;
        private readonly IDuplicateEntityPreventer<TEntity, TInterface, TId, TAltKey> _DuplicateEntityPreventer;

        public AddAltKeyHandler(IAddHandler<TEntity, TInterface, TId> addHandler,
                                IDuplicateEntityPreventer<TEntity, TInterface, TId, TAltKey> duplicateEntityPreventer)
        {
            _AddHandler = addHandler;
            _DuplicateEntityPreventer = duplicateEntityPreventer;
        }

        public async Task<List<TInterface>> AddAsync(IEnumerable<TInterface> entities)
        {
            await _DuplicateEntityPreventer.CheckAsync(entities);
            var added = await _AddHandler.AddAsync(entities);
            _DuplicateEntityPreventer.RemoveTracked(added);
            return added;
        }
    }
}