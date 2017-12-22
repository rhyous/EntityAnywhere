using Rhyous.WebFramework.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.HeaderValidators
{
    internal class TokenCache : IDictionary<string,Token>
    {
        internal Dictionary<string, Token> _Cache { get; set; } = new Dictionary<string, Token>();

        public Token this[string key] { get => ((IDictionary<string, Token>)_Cache)[key]; set => ((IDictionary<string, Token>)_Cache)[key] = value; }

        public ICollection<string> Keys => ((IDictionary<string, Token>)_Cache).Keys;

        public ICollection<Token> Values => ((IDictionary<string, Token>)_Cache).Values;

        public int Count => ((IDictionary<string, Token>)_Cache).Count;

        public bool IsReadOnly => ((IDictionary<string, Token>)_Cache).IsReadOnly;

        public void Add(string key, Token value)
        {
            ClearExpired();
            ((IDictionary<string, Token>)_Cache).Add(key, value);
        }

        public void Add(KeyValuePair<string, Token> item)
        {
            ClearExpired();
            ((IDictionary<string, Token>)_Cache).Add(item);
        }

        public void Clear()
        {
            ((IDictionary<string, Token>)_Cache).Clear();
        }

        public bool Contains(KeyValuePair<string, Token> item)
        {
            ClearExpired();
            return ((IDictionary<string, Token>)_Cache).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            ClearExpired();
            return ((IDictionary<string, Token>)_Cache).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, Token>[] array, int arrayIndex) => ((IDictionary<string, Token>)_Cache).CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<string, Token>> GetEnumerator() => ((IDictionary<string, Token>)_Cache).GetEnumerator();

        public bool Remove(string key)
        {
            ClearExpired();
            return ((IDictionary<string, Token>)_Cache).Remove(key);
        }

        public bool Remove(KeyValuePair<string, Token> item)
        {
            ClearExpired();
            return ((IDictionary<string, Token>)_Cache).Remove(item);
        }

        public bool TryGetValue(string key, out Token value)
        {
            ClearExpired();
            return ((IDictionary<string, Token>)_Cache).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, Token>)_Cache).GetEnumerator();
        }

        internal void ClearExpired()
        {
            if (_Cache.Any())
            {
                foreach (var kvp in _Cache)
                {
                    if (kvp.Value.CreateDate.AddSeconds(TokenHeaderValidator.TimeToLive) < DateTime.Now)
                        _Cache.Remove(kvp.Key);
                }
            }
        }
    }
}
