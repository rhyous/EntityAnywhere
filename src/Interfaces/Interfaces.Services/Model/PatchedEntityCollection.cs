using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Could be any object, but specifically intended to be either a
    /// TEntity or TInterface.</typeparam>
    /// <typeparam name="TId">The type of the Id property.</typeparam>
    public class PatchedEntityCollection<T, TId>
        where T : IId<TId>
    {
        public List<PatchedEntity<T, TId>> PatchedEntities
        {
            get => _PatchedEntities ?? (_PatchedEntities = new List<PatchedEntity<T, TId>>());
            set => _PatchedEntities = value;
        } private List<PatchedEntity<T, TId>> _PatchedEntities;
        /// <summary>
        /// A list of properties changed on every entity
        /// </summary>
        public HashSet<string> ChangedProperties
        {
            get { return _ChangedProperties ?? (_ChangedProperties = new HashSet<string>()); }
            set { _ChangedProperties = value; }
        } private HashSet<string> _ChangedProperties;


        public void Add(PatchedEntity<T, TId> patchedEntity)
        {
            PatchedEntities.Add(patchedEntity);
        }

        public void Add(T t)
        {
            PatchedEntities.Add(new PatchedEntity<T, TId> { Entity = t });
        }
    }
}