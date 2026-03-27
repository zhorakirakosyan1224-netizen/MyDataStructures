using System;
using System.Collections;
using System.Collections.Generic;

namespace MyBinaryTreeProj;

public class MyBinaryTree<T> : IEnumerable<T> where T : IComparable<T>
{
    private MyBinaryTreeNode<T> _head;
    private int _count;

    public int Count => _count;

    #region Add
    public void Add(T value)
    {
        var node = new MyBinaryTreeNode<T>(value);
        if (_head == null)
        {
            _head = node;
            _count++;
            return;
        }
        Add(_head, node);
    }

    private void Add(MyBinaryTreeNode<T> current, MyBinaryTreeNode<T> node)
    {
        if (node.Value.CompareTo(current.Value) < 0)
        {
            if (current.Left == null)
            {
                current.Left = node;
                _count++;
            }
            else
            {
                Add(current.Left, node);
            }
        }
        else
        {
            if (current.Right == null)
            {
                current.Right = node;
                _count++;
            }
            else
            {
                Add(current.Right, node);
            }
        }
    }
    #endregion

    #region Contains
    public bool Contains(T value)
    {
        var current = _head;
        while (current != null)
        {
            int result = current.Value.CompareTo(value);
            if (result == 0)
                return true;
            if (result > 0)
                current = current.Left;
            else
                current = current.Right;
        }
        return false;
    }
    #endregion

    #region Remove
    public bool Remove(T value)
    {
        bool removed;
        (_head, removed) = Remove(_head, value);
        if (removed)
            _count--;
        return removed;
    }

    private (MyBinaryTreeNode<T>, bool) Remove(MyBinaryTreeNode<T> node, T value)
    {
        if (node == null)
            return (null, false);

        int result = node.Value.CompareTo(value);

        if (result > 0)
        {
            (node.Left, var removed) = Remove(node.Left, value);
            return (node, removed);
        }
        else if (result < 0)
        {
            (node.Right, var removed) = Remove(node.Right, value);
            return (node, removed);
        }
        else
        {
            if (node.Left == null)
                return (node.Right, true);
            if (node.Right == null)
                return (node.Left, true);

            var successor = FindMin(node.Right);
            node = new MyBinaryTreeNode<T>(successor.Value)
            {
                Left = node.Left,
                Right = Remove(node.Right, successor.Value).Item1
            };
            return (node, true);
        }
    }

    private MyBinaryTreeNode<T> FindMin(MyBinaryTreeNode<T> node)
    {
        while (node.Left != null)
            node = node.Left;
        return node;
    }
    #endregion

    #region Enumerate
    private IEnumerable<T> InOrder(MyBinaryTreeNode<T> node)
    {
        if (node != null)
        {
            foreach (var value in InOrder(node.Left))
                yield return value;
            yield return node.Value;
            foreach (var value in InOrder(node.Right))
                yield return value;
        }
    }

    public IEnumerator<T> GetEnumerator() => InOrder(_head).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}