using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Behaviors
{
    class PluginHeaderValidator : RuntimePluginLoaderBase<IHeaderValidator>, IPluginHeaderValidator
    {
        public PluginHeaderValidator(IAppDomain appDomain,
                                     IPluginLoaderSettings settings,
                                     IPluginLoaderFactory<IHeaderValidator> pluginLoaderFactory,
                                     IPluginObjectCreator<IHeaderValidator> pluginObjectCreator,
                                     IPluginPaths pluginPaths,
                                     IPluginLoaderLogger pluginLoaderLogger)
            : base(appDomain, settings, pluginLoaderFactory, pluginObjectCreator, pluginPaths, pluginLoaderLogger)
        {
        }

        public override string PluginSubFolder => "HeaderValidators";

        /// <inheritdoc />
        public long UserId { get; set; }

        /// <inheritdoc />
        public IList<string> Headers => null;

        /// <summary>
        /// Checks if the headers authenticate to a valid user.
        /// </summary>
        /// <param name="headers">the http headers</param>
        /// <returns>Returns a long value greater than 0 if valid.</returns>
        public async Task<bool> IsValidAsync(NameValueCollection headers)
        {
            foreach (var validator in HeaderValidators)
            {
                if (!validator.CanValidateHeaders(headers.AllKeys))
                    continue;
                if (await validator.IsValidAsync(headers))
                {
                    if (validator.UserId <= 0)
                        throw new Exception("IHeaderValidator plugin must set the UserId property.");
                    UserId = validator.UserId;
                    return true;
                }
            }
            return false;
        }

        internal IEnumerable<IHeaderValidator> HeaderValidators
        {
            get { return _HeaderValidators ?? (_HeaderValidators = GetPlugins()); }
            set { _HeaderValidators = value; }
        } private IEnumerable<IHeaderValidator> _HeaderValidators;
         
        internal IList<IHeaderValidator> GetPlugins()
        {
            var plugins = CreatePluginObjects();
            if (plugins == null || plugins.Count == 0)
                throw new Exception("No HeaderValidator plugins found.");
            return plugins;
        }
    }
}