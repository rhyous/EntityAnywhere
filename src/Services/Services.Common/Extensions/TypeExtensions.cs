using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Services
{
    public static class TypeExtensions
    {
        internal static IDictionary<Type, AlternateKeyAttribute> AlternateKeyCache
        {
            get { return _AlternateKeyCache ?? (_AlternateKeyCache = new Dictionary<Type, AlternateKeyAttribute>()); }
            set { _AlternateKeyCache = value; }
        } private static IDictionary<Type, AlternateKeyAttribute> _AlternateKeyCache;


        /// <summary>
        /// A quick method to get the AlternateKey of an Entity.
        /// </summary>
        /// <param name="t">The type that is decorated with the AlternateKey attribute.</param>
        /// <returns>The name of the AlternateKey property.</returns>
        public static string GetAlternateKeyProperty(this Type t)
        {
            AlternateKeyAttribute attribute;
            if (!AlternateKeyCache.TryGetValue(t, out attribute))
            {
                attribute = t.GetCustomAttributes(typeof(AlternateKeyAttribute), true).FirstOrDefault() as AlternateKeyAttribute;
                AlternateKeyCache[t] = attribute;
            }
            return attribute?.KeyProperty;
        }

        /// <summary>
        /// A quick method to get the AdditionalTypes of an Entity.
        /// </summary>
        /// <typeparam name="T">The type of the concrete attribute that implements IAdditionalTypes.</typeparam>
        /// <param name="t">The type that is decorated with a concrete attribute that implements IAdditionalTypes interface.</param>
        /// <returns>The list of AdditionalTypes.</returns>
        public static Type[] GetAdditionalTypes<T>(this Type t)
            where T: IAdditionalTypes
        {
            var attribute = t.GetCustomAttributes(typeof(T), true).FirstOrDefault() as IAdditionalTypes;
            return attribute?.Types;
        }
    }
}
