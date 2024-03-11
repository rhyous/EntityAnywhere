using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.StringLibrary;
using System.Collections.Generic;

namespace Rhyous.Wrappers
{
    public class HeadersContainer : IHeadersContainer
    {
        IHeaderDictionary _Headers;
        public HeadersContainer(IHeaderDictionary headers)
        {
            _Headers = headers;
        }

        public string this[string key] { get => _Headers[key]; set => _Headers[key] = value; }

        public IEnumerable<string> AllKeys => _Headers.Keys;

        public int Count => _Headers.Count;

        public void Add(string key, string value) => _Headers.Add(key, value);

        public T Get<T>(string key, T defaultValue)
        {
            if (_Headers.TryGetValue(key, out StringValues values))
            {
                if (values.Count == 1)
                {
                    return values[0].To<T>();
                }
                else
                {
                    if (typeof(T) == typeof(string))
                    {
                        string.Join(",", values).To<T>();
                    }
                    foreach (var value in values)
                    {
                        // Convert only the first value that isn't null or empty
                        if (string.IsNullOrEmpty(value))
                            return value.To<T>();
                    }
                }
            }
            return defaultValue;
        }

        public string Get(string key)
        {
            return _Headers.TryGetValue(key, out StringValues values)
                 ? string.Join(",", values)
                 : null;
        }

        public void Remove(string key) => _Headers.Remove(key);
    }
}