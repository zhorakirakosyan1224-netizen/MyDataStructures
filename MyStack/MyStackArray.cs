using System;

namespace StackLib
{
    public class ArrayStack<T>
    {
        private T[] items;
        private int top;

        public ArrayStack(int capacity = 4)
        {
            items = new T[capacity];
            top = -1;
        }

        public void Push(T item)
        {
            if (top == items.Length - 1)
            {
                T[] newItems = new T[items.Length * 2];
                Array.Copy(items, newItems, items.Length);
                items = newItems;
            }
            items[++top] = item;
        }

        public T Pop()
        {
            if (top == -1)
                throw new InvalidOperationException("Stack is empty.");
            T item = items[top];
            items[top--] = default;
            return item;
        }

        public T Peek()
        {
            if (top == -1)
                throw new InvalidOperationException("Stack is empty.");
            return items[top];
        }
    }
}