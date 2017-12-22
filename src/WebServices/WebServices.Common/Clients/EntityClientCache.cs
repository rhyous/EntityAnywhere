using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Clients
{
    /// <summary>
    /// This class stores Entity Clients so we don't have to keep creating multiple instances of them.
    /// </summary>
    public class EntityClientCache : IEntityClientCache
    {
        public EntityClientCache() { UseAdminClient = false; }
        public EntityClientCache(bool useAdminClient) { UseAdminClient = useAdminClient;  }

        public bool UseAdminClient { get; set; }

        public ISharedInterfaceDictionary<string, IEntityClientBase> Generic
        {
            get { return _Generic ?? (_Generic = new EntityClientCacheGeneric()); }
            set { _Generic = value; }
        } private ISharedInterfaceDictionary<string, IEntityClientBase> _Generic;

        public IDictionaryDefaultValueProvider<string, IEntityClientAsync> Json
        {
            get { return _Json ?? (_Json = new EntityClientCacheJson(UseAdminClient)); }
            set { _Json = value; }
        } private IDictionaryDefaultValueProvider<string, IEntityClientAsync> _Json;

        #region subclasses
        public class EntityClientCacheJson : NullSafeDictionary<string, IEntityClientAsync>
        {
            public EntityClientCacheJson() { UseAdminClient = false; }
            public EntityClientCacheJson(bool useAdminClient) { UseAdminClient = useAdminClient; }

            public bool UseAdminClient { get; set; }

            public override IEntityClientAsync DefaultValueProvider(string key)
            {
                if (UseAdminClient)
                    return this[key] = new EntityClientAdminAsync(key);
                return this[key] = new EntityClientAsync(key);
            }
        }

        public class EntityClientCacheGeneric : Dictionary<string, IEntityClientBase>, ISharedInterfaceDictionary<string, IEntityClientBase>
        {
            public T GetValueOrNew<T>(string key)
                where T : class, IEntityClientBase, new()
            {
                IEntityClientBase client;
                if (!TryGetValue(key, out client) || (client as T) == null)
                {
                    var typedClient = new T();
                    this[key] = typedClient;
                    return typedClient;
                }
                return (client as T);
            }

            public T GetValueOrNew<T, TInput>(string key, TInput input, Func<TInput, T> constructor = null) 
                where T : class, IEntityClientBase, new()
            {
                IEntityClientBase client;
                if (!TryGetValue(key, out client) || (client as T) == null)
                {
                    if (constructor == null)
                        constructor = (i) => (T)Activator.CreateInstance(typeof(T), i);
                    var typedClient = constructor(input);
                    this[key] = typedClient;
                    return typedClient;
                }
                return (client as T);
            }

            public T GetValueOrNew<T, T1Input, T2Input>(string key, T1Input input1, T2Input input2, Func<T1Input, T2Input, T> constructor = null) 
                where T : class, IEntityClientBase, new()
            {
                IEntityClientBase client;
                if (!TryGetValue(key, out client) || (client as T) == null)
                {
                    if (constructor == null)
                        constructor = (i1, i2) => (T)Activator.CreateInstance(typeof(T), i1, i2);
                    var typedClient = constructor(input1, input2);
                    this[key] = typedClient;
                    return typedClient;
                }
                return (client as T);
            }
        }
        #endregion
    }
}