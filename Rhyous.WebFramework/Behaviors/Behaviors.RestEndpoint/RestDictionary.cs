using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Behaviors
{
    public class RestDictionary : Dictionary<string, string>
    {
        #region Singleton

        private static readonly Lazy<RestDictionary> Lazy = new Lazy<RestDictionary>(() => new RestDictionary());

        public static RestDictionary Instance { get { return Lazy.Value; } }

        internal RestDictionary()
        {
            Init();
        }

        #endregion

        public void Init()
        {
            Add("GetAll", "{0}"); // 0 should be pluralized entity name
            Add("GetByIds", "{0}/Ids"); // 0 should be pluralized entity name
            Add("Get", "{0}({{id}})"); // 0 should be pluralized entity name
            Add("GetProperty", "{0}({{id}})/{{property}}"); // 0 should be pluralized entity name
            Add("Post", "{0}"); // 0 should be pluralized entity name
            Add("Put", "{0}({{id}})"); // 0 should be pluralized entity name
            Add("Patch", "{0}({{id}})"); // 0 should be pluralized entity name
            Add("Delete", "{0}({{id}})"); // 0 should be pluralized entity name
            Add("GetAddenda", "{0}({{id}})/Properties"); // 0 should be pluralized entity name
            Add("GetAddendaByName", "{0}({{id}})/Properties({{name}})"); // 0 should be pluralized entity name
            Add("GetByRelatedEntityId", "{0}/?{relatedEntity}={id})"); // 0 should be pluralized entity name
        }
    }
}