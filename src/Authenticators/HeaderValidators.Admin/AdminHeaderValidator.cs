using Rhyous.WebFramework.Interfaces;
using System.Collections.Specialized;
using System.Configuration;

namespace Rhyous.WebFramework.HeaderValidators
{
    /// <summary>
    /// This plugin is used to allow calls to Entity Web Services without creating an infinite loop.
    /// </summary>
    public class AdminHeaderValidator : IHeaderValidator
    {
        /// <inheritdoc />
        public long UserId { get; set; }

        /// <inheritdoc />
        public bool IsValid(NameValueCollection headers)
        {
            var adminToken = ConfigurationManager.AppSettings.Get("EntityAdminToken", "");
            if (headers.Get("EntityAdminToken") == adminToken)
            {
                UserId = 1;
                return true;
            }
            return false;
        }
    }
}
