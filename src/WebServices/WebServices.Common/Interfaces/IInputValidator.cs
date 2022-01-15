using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IInputValidator<TEntity, TId>
        where TEntity : IId<TId>
    {
        bool CleanAndValidate(Type type, ref string id, ref string property, ref string value);
        bool CleanAndValidate(ref string id, PatchedEntity<TEntity, TId> patchedEntity);
        bool CleanAndValidate(PatchedEntityCollection<TEntity, TId> patchedEntityCollection);
        bool CleanAndValidate(ref string id, TEntity entity);
    }
}