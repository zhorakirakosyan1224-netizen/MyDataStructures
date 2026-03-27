using System;
using System.Collections.Generic;
using System.Text;

namespace MyLinkedListLib
{
    public class MyLinkedListNode<T>
    {
        public T Value { get; set; }
        public MyLinkedListNode<T> Next { get; set; }

        public MyLinkedListNode(T value, MyLinkedListNode<T> next)
        {
            Value = value;
            Next = next;
        }
    }
}