using System.Collections;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Interfaces
{
    public class ParentedCollection<T, TParent> : ICollection<T>
        where T : IParent<TParent>
    {
        internal readonly ICollection<T> _List = new List<T>();

        public ParentedCollection() { }
        public ParentedCollection(TParent parent) { Parent = parent; }

        protected TParent Parent { get; set; }

        public int Count => _List.Count;

        public bool IsReadOnly => _List.IsReadOnly;

        public void Add(T item)
        {
            _List.Add(item);
            (item as IParent<TParent>).Parent = Parent;
        }

        public void Clear() => _List.Clear();

        public bool Contains(T item) => _List.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _List.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _List.GetEnumerator();

        public bool Remove(T item) => _List.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}