using System;
using System.Collections;
using System.Collections.Generic;
using MyLinkedListLib;

namespace MyQueueLib
{
    public class MyQueue<T> : IEnumerable<T>
    {
        private MyLinkedList<T> items;

        public MyQueue()
        {
            items = new MyLinkedList<T>();
        }

        public int Count => items.Count;

        public void Enqueue(T item)
        {
            items.AddLast(item);
        }

        public T Dequeue()
        {
            if (items.Count == 0)
                throw new InvalidOperationException("Queue is empty.");
            T value = items.Head.Value;
            items.RemoveFirst();
            return value;
        }

        public T Peek()
        {
            if (items.Count == 0)
                throw new InvalidOperationException("Queue is empty.");
            return items.Head.Value;
        }

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}