using Rhyous.WebFramework.Interfaces;
using System.Collections.Specialized;

namespace Rhyous.WebFramework.HeaderValidators
{
    /// <summary>
    /// This plugin is intended to be used only in debug mode to allow any service response without having to authenticate.
    /// It is probably best you don't deploy this plugin to production.
    /// </summary>
    public class NoAuthHeaderValidator : IHeaderValidator
    {
        /// <inheritdoc />
        public long UserId { get; set; }

        /// <inheritdoc />
        public bool IsValid(NameValueCollection headers)
        {
            return true;
        }
    }
}
