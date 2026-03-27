using MyLinkedListLib;

namespace StackLib
{
    public class MyStack<T>
    {
        private MyLinkedList<T> items = new MyLinkedList<T>();

        public int Count => items.Count;
        public bool IsEmpty => items.Count == 0;

        public void Push(T item)
        {
            items.AddFirst(item);
        }

        public T Pop()
        {
            T item = items.First();
            items.RemoveFirst();
            return item;
        }

        public T Peek()
        {
            return items.First();
        }
    }
}