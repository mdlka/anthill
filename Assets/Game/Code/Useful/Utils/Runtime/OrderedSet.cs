using System;
using System.Collections;
using System.Collections.Generic;

namespace YellowSquad.Utils
{
    public sealed class OrderedSet<T> : ICollection<T>
    {
        private readonly IDictionary<T, LinkedListNode<T>> m_Dictionary;
        private readonly LinkedList<T> m_LinkedList;

        public OrderedSet() : this(EqualityComparer<T>.Default) {}

        public OrderedSet(IEqualityComparer<T> comparer)
        {
            m_Dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
            m_LinkedList = new LinkedList<T>();
        }

        public int Count => m_Dictionary.Count;
        public bool IsReadOnly => m_Dictionary.IsReadOnly;

        public bool Add(T item)
        {
            if (m_Dictionary.ContainsKey(item)) 
                return false;
            
            LinkedListNode<T> node = m_LinkedList.AddLast(item);
            m_Dictionary.Add(item, node);
            return true;
        }
        
        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public void Clear()
        {
            m_LinkedList.Clear();
            m_Dictionary.Clear();
        }

        public bool Remove(T item)
        {
            bool found = m_Dictionary.TryGetValue(item, out LinkedListNode<T> node);
            
            if (!found) 
                return false;
            
            m_Dictionary.Remove(item);
            m_LinkedList.Remove(node);
            return true;
        }
        
        public void RemoveWhere(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException();

            for (LinkedListNode<T> node = m_LinkedList.First; node != null; node = node.Next == m_LinkedList.First ? null : node.Next)
            {
                if (match(node.Value))
                {
                    m_LinkedList.Remove(node);
                    m_Dictionary.Remove(node.Value);
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_LinkedList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(T item)
        {
            return m_Dictionary.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_LinkedList.CopyTo(array, arrayIndex);
        }
    }
}