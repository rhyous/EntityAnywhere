using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Clients2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class IdDisambiguator<TEntity, TId>
                 : IdDisambiguator<TEntity, TId, string>,
                   IIdDisambiguator<TEntity, TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        new public DisambiguatedId<TId> Disambiguate(string ambiguousId)
        {
            var result = base.Disambiguate(ambiguousId);
            return new DisambiguatedId<TId>
            {
                Id = result.Id,
                IdType = result.IdType,
                AltId = result.AltId,
                OrginalId = result.OrginalId,
                AlternateIdProperty = result.AlternateIdProperty
            };
        }
    };

    public class IdDisambiguator<TEntity, TId, TAltKey> : IIdDisambiguator<TEntity, TId, TAltKey>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public DisambiguatedId<TId, TAltKey> Disambiguate(string ambiguousId)
        {
            if (string.IsNullOrWhiteSpace(ambiguousId))
                return null;
            var disambiguated = new DisambiguatedId<TId, TAltKey> { OrginalId = ambiguousId };
            if (!ambiguousId.StartsWith(IdDisambiguator.Prefix)) // If there is no prefix, treat it as if it were an $Id.
                return Actions[IdDisambiguator.Id].Invoke(new[] { IdDisambiguator.Id, ambiguousId }, disambiguated);
            var idParts = ambiguousId.Split(new[] { IdDisambiguator.Separator }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (idParts?.Length == 1)
                return Actions[IdDisambiguator.Id].Invoke(new[] { IdDisambiguator.Id, ambiguousId }, disambiguated);
            if (!(idParts.Length == 2 || idParts.Length == 3))
                throw new InvalidEntityIdException(typeof(TEntity).Name, ambiguousId);
            disambiguated.IdType = (IdType)Enum.Parse(typeof(IdType), idParts[0].Trim('$'), true);
            return Actions[idParts[0]].Invoke(idParts, disambiguated);
        }

        public Dictionary<string, Func<string[], DisambiguatedId<TId, TAltKey>, DisambiguatedId<TId, TAltKey>>> Actions { get { return _Actions ?? (_Actions = BuildActionDictionary()); } }
        Dictionary<string, Func<string[], DisambiguatedId<TId, TAltKey>, DisambiguatedId<TId, TAltKey>>> _Actions;

        internal Dictionary<string, Func<string[], DisambiguatedId<TId, TAltKey>, DisambiguatedId<TId, TAltKey>>> BuildActionDictionary()
        {
            return new Dictionary<string, Func<string[], DisambiguatedId<TId, TAltKey>, DisambiguatedId<TId, TAltKey>>>(StringComparer.OrdinalIgnoreCase)
            {
                { IdDisambiguator.Id, HandleIdKeyword },
                { IdDisambiguator.Alt, HandleAltKeyword }
            };
        }

        internal DisambiguatedId<TId, TAltKey> HandleIdKeyword(string[] idParts, DisambiguatedId<TId, TAltKey> disambiguatedId)
        {
            if (idParts == null)
                throw new InvalidEntityIdException(typeof(TEntity).Name, null);
            if (idParts.Length != 2 || idParts.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new InvalidEntityIdException(typeof(TEntity).Name, string.Join(IdDisambiguator.Separator.ToString(), idParts));
            disambiguatedId.AltId = idParts[1].To<TAltKey>();
            disambiguatedId.Id = idParts[1].To<TId>();
            if (disambiguatedId.Id.Equals(default(TId))) // If it doesn't cast, it must be an AlternateKey or fail
            {
                if (!typeof(TEntity).GetCustomAttributes(true).Any(a => a is AlternateKeyAttribute))
                    throw new InvalidEntityIdException(typeof(TEntity).Name, string.Join(IdDisambiguator.Separator.ToString(), idParts));
                disambiguatedId.IdType = IdType.Alt;
                disambiguatedId.AlternateIdProperty = IdDisambiguator.Key;
            }
            return disambiguatedId;
        }

        internal DisambiguatedId<TId, TAltKey> HandleAltKeyword(string[] idParts, DisambiguatedId<TId, TAltKey> disambiguatedId)
        {
            if (idParts == null)
                throw new InvalidEntityIdException(typeof(TEntity).Name, null);
            if (idParts.Length != 3 || idParts.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new InvalidEntityIdException(typeof(TEntity).Name, string.Join(IdDisambiguator.Separator.ToString(), idParts));
            disambiguatedId.AlternateIdProperty = idParts[1];
            disambiguatedId.AltId = idParts[2].To<TAltKey>();
            return disambiguatedId;
        }
    }
}