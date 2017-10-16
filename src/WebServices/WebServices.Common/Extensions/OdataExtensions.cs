using Rhyous.StringLibrary;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This extension class makes wrapping objects in Odata types easier.
    /// </summary>
    public static class OdataExtensions        
    {
        public const string ObjectUrl = "{0}({1})";
        public const string IdProperty = "Id";

        public static OdataObject<T> AsOdata<T>(this T t, string leftPartOfUrl, string idProperty, params string[] properties)
        {
            return t.AsOdata(leftPartOfUrl, IdProperty, true, properties);
        }

        public static OdataObject<T> AsOdata<T>(this T t, string leftPartOfUrl, string idProperty, bool addIdToUrl, params string[] properties)
        {
            var obj = new OdataObject<T> { Object = t, PropertyUris = new List<ODataUri>(), RelatedEntities = new List<string> { "{ Name : \"MyName\" }" } };
            if (!string.IsNullOrWhiteSpace(leftPartOfUrl))
                obj.Uri = addIdToUrl
                        ? new Uri(string.Format(ObjectUrl, leftPartOfUrl, t.GetPropertyValue(idProperty)))
                        : new Uri(leftPartOfUrl);
            // Uncomment below if we decide to publish all Entity properties
            //if (properties == null || properties.Length == 0)
            //    properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => !p.CustomAttributes.Any(a => a.AttributeType == typeof(IgnoreDataMemberAttribute)))?.Select(p=>p.Name).ToArray();
            AddPropertyUris(properties, obj);
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
                    Uri = new Uri("/" + prop, UriKind.Relative)
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
            var leftPart = uri?.GetLeftPart(UriPartial.Path);
            return ts.Select(t => t.AsOdata(leftPart, true, properties)).ToList();
        }

        public static List<OdataObject<T>> AsOdata<T, TId>(this IEnumerable<T> list, Uri uri, List<Addendum> addenda, params string[] properties)
            where T : IId<TId>
        {
            var entity = typeof(T).Name;
            var odataList = new List<OdataObject<T>>();
            foreach (T e in list)
                odataList.Add(e.AsOdata<T>(uri, addenda.Where(a => a.Entity == entity && a.EntityId == e.Id.ToString()).ToList()));
            return odataList;
        }

        public static OdataObject<T> AsOdata<T>(this T t, Uri uri, List<Addendum> addenda, params string[] properties)
        {
            var leftPart = uri.GetLeftPart(UriPartial.Path);            
            var odata = t.AsOdata<T>(leftPart, false, properties);
            odata.Addenda = addenda;
            return odata;
        }

        private static void AddPropertyUris<T>(string[] properties, OdataObject<T> obj)
        {
            if (properties != null)
            {
                foreach (var prop in properties)
                    obj.AddProperty(prop);
            }
        }
    }
}
