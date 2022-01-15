using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IIdDisambiguator<TEntity, TId, TAltKey>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        DisambiguatedId<TId, TAltKey> Disambiguate(string ambiguousId);
    }

    public interface IIdDisambiguator<TEntity, TId> : IIdDisambiguator<TEntity, TId, string>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        new DisambiguatedId<TId> Disambiguate(string ambiguousId);
    }
}
