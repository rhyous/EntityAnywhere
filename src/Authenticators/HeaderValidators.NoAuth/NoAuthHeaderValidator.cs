using Rhyous.WebFramework.Interfaces;
using System.Collections.Specialized;
using System;

namespace Rhyous.WebFramework.HeaderValidators
{
    /// <summary>
    /// This is useful in debug mode to allow any service response
    /// without having to authenticate. It is probably best you
    /// don't deploy this plugin to production.
    /// </summary>
    public class NoAuthHeaderValidator : IHeaderValidator
    {
        public long UserId { get; set; }

        public bool IsValid(NameValueCollection headers)
        {
            return true;
        }
    }
}
