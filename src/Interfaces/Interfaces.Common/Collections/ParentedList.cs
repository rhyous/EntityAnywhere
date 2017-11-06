using System.Collections;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Interfaces
{
    public class ParentedList<T, TParent> : IList<T>
        where T : IParent<TParent>
    {
        internal readonly IList<T> _List = new List<T>();

        public ParentedList() { }
        public ParentedList(TParent parent) { Parent = parent; }

        protected TParent Parent { get; set; }

        public int Count => _List.Count;

        public bool IsReadOnly => _List.IsReadOnly;

        public T this[int index] { get { return _List[index]; } set { _List[index] = value; } }

        public void Add(T item)
        {
            _List.Add(item);
            (item as IParent<TParent>).Parent = Parent;
        }

        public void AddRange(IEnumerable<T> items)
        {
            ((List<T>)_List).AddRange(items);
            foreach (var item in items)
                (item as IParent<TParent>).Parent = Parent;
        }

        public void Clear() => _List.Clear();

        public bool Contains(T item) => _List.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _List.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _List.GetEnumerator();

        public bool Remove(T item) => _List.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item)
        {
            return _List.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _List.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _List.RemoveAt(index);
        }
    }
}