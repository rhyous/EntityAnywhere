using Rhyous.StringLibrary;
using System;

namespace Rhyous.EntityAnywhere.Clients2
{
    public abstract partial class EntityClientBase : IEntityClientBase
    {
        protected readonly IEntityClientConnectionSettings _EntityClientConnectionSettings;

        public EntityClientBase(IEntityClientConnectionSettings entityClientConnectionSettings)
        {
            _EntityClientConnectionSettings = entityClientConnectionSettings ?? throw new ArgumentNullException(nameof(entityClientConnectionSettings));
        }

        /// <inheritdoc />
        public virtual string Entity => _EntityClientConnectionSettings.EntityName;

        public virtual string EntityPluralized => _EntityClientConnectionSettings.EntityNamePluralized;

        public virtual string ServiceUrl => _EntityClientConnectionSettings.ServiceUrl;

        public static string AppendUrlParameters(string urlParameters, string url)
        {
            if (!string.IsNullOrWhiteSpace(urlParameters))
                return StringConcat.WithSeparator('?', url, urlParameters);
            return url;
        }
    }
}