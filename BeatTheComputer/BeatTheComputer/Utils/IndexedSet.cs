using System.Collections.Generic;
using System.Collections;

namespace BeatTheComputer.Utils
{
    class IndexedSet<T> : ICollection<T>, IEnumerable<T>, IList<T>
    {
        List<T> list;
        Dictionary<T, int> itemsToIndices;

        public IndexedSet()
        {
            list = new List<T>();
            itemsToIndices = new Dictionary<T, int>();
        }

        public IndexedSet(IEnumerable<T> collection)
        {
            list = new List<T>(collection);
            itemsToIndices = new Dictionary<T, int>();
            updateIndices(0, Count);
        }

        public void Add(T item)
        {
            if (!itemsToIndices.ContainsKey(item)) {
                list.Add(item);
                itemsToIndices.Add(item, list.Count - 1);
            }
        }

        public void Insert(int index, T item)
        {
            if (!itemsToIndices.ContainsKey(item)) {
                list.Insert(index, item);
                updateIndices(index, Count);
            }
        }

        public bool Contains(T item) { return itemsToIndices.ContainsKey(item); }

        public int IndexOf(T item)
        {
            int index;
            if (!itemsToIndices.TryGetValue(item, out index)) {
                return -1;
            }
            return index;
        }

        public void Clear()
        {
            list.Clear();
            itemsToIndices.Clear();
        }

        public void CopyTo(T[] array, int arrayIndex) { list.CopyTo(array, arrayIndex); }

        public bool Remove(T item)
        {
            int removeIndex;
            if (itemsToIndices.TryGetValue(item, out removeIndex)) {
                itemsToIndices.Remove(item);
                list.RemoveAt(removeIndex);
                updateIndices(removeIndex, Count);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < Count) {
                itemsToIndices.Remove(list[index]);
                list.RemoveAt(index);
                updateIndices(index, Count);
            }
        }

        public T this[int index] {
            get { return list[index]; }
            set {
                if (!itemsToIndices.ContainsKey(value)) {
                    itemsToIndices.Remove(list[index]);
                    list[index] = value;
                    itemsToIndices.Add(value, index);
                }
            }
        }

        public int Count {
            get { return list.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public IEnumerator<T> GetEnumerator() { return list.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return list.GetEnumerator(); }

        private void updateIndices(int begin, int end)
        {
            for (int i = begin; i < end; i++) {
                itemsToIndices[list[i]] = i;
            }
        }
    }
}
