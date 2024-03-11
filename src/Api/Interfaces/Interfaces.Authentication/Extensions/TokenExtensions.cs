using Rhyous.StringLibrary;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public static class TokenExtensions
    {
        /// <summary>
        /// Takes a concrete class acting as the EntityInterface and converts it to act as type T.
        /// </summary>
        /// <typeparam name="T">The destination concrete type of EntityInterface. It must be a class. It must implement Tinterface. It must have a generic constructor.</typeparam>
        /// <param name="items">An IEnumerable{EntityInterface}.</param>
        /// <returns>Returns a new IEnumerable{T}. The items are the same if the source concrete class is the same as the destination type T. If the destination type is different, then a shallow copy is returned where all public properties of EntityInterface, including inherited properties, are copied.</returns>
        public static IEnumerable<T> ToConcrete<T>(this IEnumerable<IToken> items)
            where T : class, IToken, new()
        {
            return items.Select(i => i.ToConcrete<T>()).ToList();
        }

        /// <summary>
        /// Takes a concrete class acting as the EntityInterface and converts it to act as type T.
        /// </summary>
        /// <typeparam name="T">The destination concrete type of Tinterface. It must be a class. It must implement Tinterface. It must have a generic constructor.</typeparam>
        /// <param name="item"></param>
        /// <returns>The same instance passed in, if the source concrete class is the same as the destination type T. If the destination type is different, then a shallow copy is returned where all public properties of EntityInterface, including inherited properties, are copied.</returns>
        public static T ToConcrete<T>(this IToken item)
            where T : class, IToken, new()
        {
            return ConcreteConverter.ToConcrete<T, IToken>(item);
        }

        /// <summary>
        /// Gets claim value from a token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="domainSubject"></param>
        /// <param name="claimName"></param>
        /// <returns>List of strings</returns>
        public static string GetClaimValue(this IToken token, string domainSubject, string claimName)
        {
            return token?.ClaimDomains?.FirstOrDefault(c => c.Subject == domainSubject)?
                                       .Claims?.FirstOrDefault(c => c.Name == claimName)?
                                       .Value;
        }

        /// <summary>
        /// Gets claim value from a token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="domainSubject"></param>
        /// <param name="claimSubject"></param>
        /// <returns>List of strings</returns>
        public static T GetClaimValue<T>(this IToken token, string domainSubject, string claimName)
        {
            var value = token.GetClaimValue(domainSubject, claimName);
            return string.IsNullOrWhiteSpace(value)
                   ? default
                   : value.To<T>();
        }
    }
}