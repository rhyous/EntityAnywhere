using Rhyous.Odata;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This extension class makes wrapping objects in Odata types easier. This is a copy of the one in Rhyous.Odata,
    /// only it returns the local inherited version of OdataObject<![CDATA[<T,TId>]]>.
    /// </summary>
    public static class OdataExtensions
    {
        public const string ObjectUrl = "{0}({1})";

        #region single entity
        public static OdataObject<T, TId> AsOdata<T, TId>(this T t, string leftPartOfUrl, bool addIdToUrl, UriKind uriKind = UriKind.Relative, params string[] properties)
        {
            var obj = new OdataObject<T, TId> { Object = t, PropertyUris = new List<OdataUri>() };
            obj.SetUri(leftPartOfUrl, uriKind, addIdToUrl);
            // Uncomment below if we decide to publish all Entity property uris
            //if (properties == null || properties.Length == 0)
            //    properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => !p.CustomAttributes.Any(a => a.AttributeType == typeof(IgnoreDataMemberAttribute)))?.Select(p=>p.Name).ToArray();
            obj.AddPropertyUris(properties);
            return obj;
        }

        public static OdataObject<T, TId> AsOdata<T, TId>(this T t, Uri uri, List<Addendum> addenda = null, params string[] properties)
        {
            var leftPart = uri.GetLeftPart(UriPartial.Path);
            var uriKind = uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative;
            var odata = t.AsOdata<T, TId>(leftPart, false, uriKind, properties);
            odata.Addenda = addenda;
            return odata;
        }
        #endregion

        #region multiple entities
        public static List<OdataObject<T, TId>> AsOdata<T, TId>(this IEnumerable<T> list, Uri uri, List<Addendum> addenda = null, params string[] properties)
        {
            var entity = typeof(T).Name;
            var odataList = new List<OdataObject<T, TId>>();
            foreach (T e in list)
                odataList.Add(e.AsOdata<T, TId>(uri, addenda?.Where(a => a.Entity == entity && a.EntityId == e.GetPropertyValue("Id").ToString()).ToList()));
            return odataList;
        }
        #endregion

        #region internal
        private static void SetUri<T, TId>(this OdataObject<T, TId> obj, string leftPartOfUrl, UriKind uriKind, bool addIdToUrl)
        {
            if (!string.IsNullOrWhiteSpace(leftPartOfUrl))
                obj.Uri = addIdToUrl
                        ? new Uri(string.Format(ObjectUrl, leftPartOfUrl, obj.Id), uriKind)
                        : new Uri(leftPartOfUrl, uriKind);
        }

        internal static void AddPropertyUris<T, TId>(this OdataObject<T, TId> obj, string[] properties)
        {
            if (properties != null)
            {
                foreach (var prop in properties)
                    obj.AddProperty(prop);
            }
        }

        internal static void AddProperty<T, TId>(this OdataObject<T, TId> obj, string prop)
        {
            obj.PropertyUris.Add(
                new OdataUri
                {
                    PropertyName = prop,
                    Uri = new Uri("/" + prop, UriKind.Relative)
                }
            );
        }
        #endregion
    }
}