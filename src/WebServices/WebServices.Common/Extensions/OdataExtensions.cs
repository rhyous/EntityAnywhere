using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    public static class OdataExtensions
    {
        public const string ObjectUrl = "{0}({1})";
        public const string PropertyUrl = "{0}/{1}";
        public const string IdProperty = "Id";

        public static OdataObject<T> AsOdata<T>(this T t, string leftPartOfUrl, string idProperty, params string[] properties)
        {
            return t.AsOdata(leftPartOfUrl, IdProperty, true, properties);
        }

        public static OdataObject<T> AsOdata<T>(this T t, string leftPartOfUrl, string idProperty, bool addIdToUrl, params string[] properties)
        {
            var obj = new OdataObject<T> { Object = t, PropertyUris = new List<ODataUri>() };
            obj.Uri = addIdToUrl ? new Uri(string.Format(ObjectUrl, leftPartOfUrl, t.GetPropertyValue(idProperty))) : new Uri(leftPartOfUrl);
            if (properties != null)
            {
                foreach (var prop in properties)
                {
                    obj.AddProperty(prop);
                }
            }
            return obj;
        }

        public static OdataObject<T> AsOdata<T>(this T t, string leftPartOfUrl, params string[] properties)
        {
            return t.AsOdata(leftPartOfUrl, IdProperty, properties);
        }

        private static void AddProperty<T>(this OdataObject<T> obj, string prop)
        {
            obj.PropertyUris.Add(
                new ODataUri
                {
                    PropertyName = prop,
                    Uri = new Uri(string.Format(PropertyUrl, obj.Uri, prop))
                }
            );
        }

        public static OdataObject<T> AsOdata<T>(this T t, Uri uri, params string[] properties)
        {
            var leftPart = uri.GetLeftPart(UriPartial.Path);
            return t.AsOdata<T>(leftPart, false, properties);
        }

        public static OdataObject<T> AsOdata<T>(this T t, string leftPart, bool addIdToUrl, params string[] properties)
        {
            var id = t.GetPropertyValue(IdProperty);
            return t.AsOdata(leftPart, IdProperty, addIdToUrl, properties);
        }

        public static List<OdataObject<T>> AsOdata<T>(this List<T> ts, Uri uri,  params string[] properties)
        {
            var leftPart = uri.GetLeftPart(UriPartial.Path);
            return ts.Select(t => t.AsOdata(leftPart, true, properties)).ToList();
        }

        public static OdataObject<T> AsOdata<T>(this T t, Uri uri, List<Addendum> addenda, params string[] properties)
        {
            var leftPart = uri.GetLeftPart(UriPartial.Path);            
            var odata = t.AsOdata<T>(leftPart, false, properties);
            odata.Addenda = addenda;
            return odata;
        }
    }
}
