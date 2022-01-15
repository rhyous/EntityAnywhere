using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// This is the internal service for RES Activation. It decrypts and deserializes the activation
    /// request, then it uses the product information to determine if the request should be 
    /// processed in ILS or Cypher.
    /// </summary>
    public interface IResActivationService : IServiceCommonAlternateKey<ResActivation, IResActivation, int, Guid>
    {
        Task<string> AutomaticActivationAsync(string activationRequest, bool FIPS = false);

        Task<Stream> ManualActivationAsync(string activationRequest, string licenseGuid, bool FIPS = false);

    }
}
