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

        public static OdataObject<T, TId> AsOdata<T, TId>(this T t, string leftPartOfUrl, string idProperty, params string[] properties)
            where T : IId<TId>
        {
            return t.AsOdata<T, TId>(leftPartOfUrl, IdProperty, true, properties);
        }

        public static OdataObject<T, TId> AsOdata<T, TId>(this T t, string leftPartOfUrl, string idProperty, bool addIdToUrl, params string[] properties)
            where T : IId<TId>
        {
            var obj = new OdataObject<T, TId> { Id = t.Id, Object = t, PropertyUris = new List<ODataUri>() };
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

        public static OdataObject<T, TId> AsOdata<T, TId>(this T t, string leftPartOfUrl, params string[] properties)
            where T : IId<TId>
        {
            return t.AsOdata<T, TId>(leftPartOfUrl, IdProperty, properties);
        }

        private static void AddProperty<T, TId>(this OdataObject<T, TId> obj, string prop)
            where T : IId<TId>
        {
            obj.PropertyUris.Add(
                new ODataUri
                {
                    PropertyName = prop,
                    Uri = new Uri("/" + prop, UriKind.Relative)
                }
            );
        }

        public static OdataObject<T, TId> AsOdata<T, TId>(this T t, Uri uri, params string[] properties)
            where T : IId<TId>
        {
            var leftPart = uri.GetLeftPart(UriPartial.Path);
            return t.AsOdata<T, TId>(leftPart, false, properties);
        }

        public static OdataObject<T, TId> AsOdata<T, TId>(this T t, string leftPart, bool addIdToUrl, params string[] properties)
            where T : IId<TId>
        {
            var id = t.GetPropertyValue(IdProperty);
            return t.AsOdata<T, TId>(leftPart, IdProperty, addIdToUrl, properties);
        }

        public static List<OdataObject<T, TId>> AsOdata<T, TId>(this List<T> ts, Uri uri,  params string[] properties)
            where T : IId<TId>
        {
            var leftPart = uri?.GetLeftPart(UriPartial.Path);
            return ts.Select(t => t.AsOdata<T, TId>(leftPart, true, properties)).ToList();
        }

        public static List<OdataObject<T, TId>> AsOdata<T, TId>(this IEnumerable<T> list, Uri uri, List<Addendum> addenda, params string[] properties)
            where T : IId<TId>
        {
            var entity = typeof(T).Name;
            var odataList = new List<OdataObject<T, TId>>();
            foreach (T e in list)
                odataList.Add(e.AsOdata<T, TId>(uri, addenda.Where(a => a.Entity == entity && a.EntityId == e.Id.ToString()).ToList()));
            return odataList;
        }

        public static OdataObject<T, TId> AsOdata<T, TId>(this T t, Uri uri, List<Addendum> addenda, params string[] properties)
            where T : IId<TId>
        {
            var leftPart = uri.GetLeftPart(UriPartial.Path);            
            var odata = t.AsOdata<T, TId>(leftPart, false, properties);
            odata.Addenda = addenda;
            return odata;
        }

        private static void AddPropertyUris<T, TId>(string[] properties, OdataObject<T, TId> obj)
            where T : IId<TId>
        {
            if (properties != null)
            {
                foreach (var prop in properties)
                    obj.AddProperty(prop);
            }
        }
    }
}