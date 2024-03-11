using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IHeadersContainer
    {
        void Add(string key, string value);
        IEnumerable<string> AllKeys { get; }
        T Get<T>(string key, T defaultValue);
        string Get(string key);
        void Remove(string key);
        string this[string key] { get; }
        int Count { get; }
    }
}