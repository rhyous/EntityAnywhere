using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services
{
    public static class CredentialsValidatorResponseExtensions
    {
        public static CredentialsValidatorResponse MergeFailed(this List<CredentialsValidatorResponse> responses)
        {
            return new CredentialsValidatorResponse
            {
                Success = false,
                AuthenticationPlugin = string.Join(", ", responses.Select(r => r.AuthenticationPlugin)),
                Message = string.Join(", ", responses.Select(r => $"{r.AuthenticationPlugin}:{r.Message}")),
                Token = null
            };
        }
    }
}
