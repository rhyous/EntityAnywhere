using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public static class HeaderValidatorExtensions
    {
        public static bool CanValidateHeaders(this IHeaderValidator validator, IEnumerable<string> headerKeys)
        {
            var headers = validator.Headers;
            return validator != null 
                && (headers == null 
                || !headers.Any() 
                || (headerKeys != null && headerKeys.Intersect(headers, StringComparer.OrdinalIgnoreCase).Any()));
        }
    }
}