using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{

    /// <summary>
    /// This loads the credential validator plugins.
    /// </summary>
    public class PluginCredentialsValidator : RuntimePluginLoaderBase<ICredentialsValidatorAsync>, ICredentialsValidatorLoader
    {
        private const string Any = nameof(Any);

        public ILogger Logger { get; set; }

        public PluginCredentialsValidator(IAppDomain appDomain,
                                          IPluginLoaderSettings settings,
                                          IPluginLoaderFactory<ICredentialsValidatorAsync> pluginLoaderFactory,
                                          IPluginObjectCreator<ICredentialsValidatorAsync> pluginObjectCreator,
                                          IPluginPaths pluginPaths = null,
                                          IPluginLoaderLogger pluginLoaderLogger = null,
                                          ILogger logger = null)
            : base(appDomain, settings, pluginLoaderFactory, pluginObjectCreator, pluginPaths, pluginLoaderLogger ?? new PluginLoaderLoggerWrapper(logger))
        {
            Logger = logger;
        }

        public string Name => nameof(PluginCredentialsValidator);

        public override string PluginSubFolder => "Authenticators";

        public async Task<CredentialsValidatorResponse> IsValidAsync(ICredentials creds)
        {
            var plugins = CreatePluginObjects();

            if (creds.AuthenticationPlugin != Any)
            {
                var plugin = plugins.FirstOrDefault(p => p.Name.Equals(creds.AuthenticationPlugin, StringComparison.OrdinalIgnoreCase));
                return await IsValidAsyncTryCatch(plugin.IsValidAsync, creds);
            }

            var authTasks = plugins.Select(p => IsValidAsyncTryCatch(p.IsValidAsync, creds)).ToList();
            var responses = new List<CredentialsValidatorResponse>(); // Won't be used if any one plugin succeeds
            while (authTasks.Any())
            {
                var authTask = await Task.WhenAny(authTasks.ToArray());
                if (authTask.Result != null && authTask.Result.Success)
                    return authTask.Result;
                authTasks.Remove(authTask);
                responses.Add(authTask.Result);
            }
            // All failed to get here
            return responses.MergeFailed();
        }

        public async Task<CredentialsValidatorResponse> IsValidAsyncTryCatch(Func<ICredentials, Task<CredentialsValidatorResponse>> myMethod, ICredentials creds)
        {
            try
            {
                return await myMethod(creds);
            }
            catch(Exception ex)
            {
                Logger?.Debug(ex.Message);
                return null; // If we try multiple plugins, we don't care if one fails as long as one succeeds.
            }
        }
    }
}