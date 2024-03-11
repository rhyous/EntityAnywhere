using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rhyous.Wrappers
{
    public class HeadersContainer : IHeadersContainer
    {
        NameValueCollection _Headers;
        public HeadersContainer(NameValueCollection headers)
        {
            _Headers = headers;
        }

        public string this[string key] { get => _Headers[key]; set => _Headers[key] = value; }

        public IEnumerable<string> AllKeys => _Headers.AllKeys;

        public int Count => _Headers.Count;

        public void Add(string key, string value) => _Headers.Add(key, value);

        public T Get<T>(string key, T defaultValue) => _Headers.Get(key, defaultValue);

        public string Get(string key) => _Headers.Get(key);

        public void Remove(string key) => _Headers.Remove(key);
    }
}