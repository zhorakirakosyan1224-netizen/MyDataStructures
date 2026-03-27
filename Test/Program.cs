using System;
using System.Linq;
using MyLinkedListLib;
using StackLib;
using MyQueueLib;
using MyBinaryTreeProj;

namespace Tests
{
    internal class Program
    {
        static int passed = 0;
        static int failed = 0;

        static void Main(string[] args)
        {
            TestLinkedList();
            TestLinkedListStack();
            TestArrayStack();
            TestQueue();
            TestBinaryTree();

            Console.WriteLine();
            Console.WriteLine("================================");
            Console.WriteLine($"  Results: {passed} passed, {failed} failed");
            Console.WriteLine("================================");
        }

        #region Helpers
        static void Assert(string testName, bool condition)
        {
            if (condition)
            {
                Console.WriteLine($"  [PASS] {testName}");
                passed++;
            }
            else
            {
                Console.WriteLine($"  [FAIL] {testName}");
                failed++;
            }
        }

        static void AssertThrows<TException>(string testName, Action action) where TException : Exception
        {
            try
            {
                action();
                Console.WriteLine($"  [FAIL] {testName} (no exception thrown)");
                failed++;
            }
            catch (TException)
            {
                Console.WriteLine($"  [PASS] {testName}");
                passed++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [FAIL] {testName} (wrong exception: {ex.GetType().Name})");
                failed++;
            }
        }

        static void Section(string name)
        {
            Console.WriteLine();
            Console.WriteLine($"--- {name} ---");
        }
        #endregion

        #region LinkedList Tests
        static void TestLinkedList()
        {
            Section("MyLinkedList");

            var list = new MyLinkedList<int>();

            list.AddLast(42);
            list.AddLast(17);
            list.AddLast(85);
            Assert("Appending to back: total node count equals 3", list.Count == 3);
            Assert("Appending to back: first node holds 42", list.Head.Value == 42);
            Assert("Appending to back: last node holds 85", list.Tail.Value == 85);

            list.AddFirst(9);
            Assert("Prepending to front: first node is now 9", list.Head.Value == 9);
            Assert("Prepending to front: total node count equals 4", list.Count == 4);

            Assert("Search: 17 is found in the list", list.Contains(17));
            Assert("Search: 63 is not present in the list", !list.Contains(63));

            list.Remove(9);
            Assert("Deleting front node: head is now 42", list.Head.Value == 42);
            Assert("Deleting front node: total node count equals 3", list.Count == 3);

            list.Remove(85);
            Assert("Deleting back node: tail is now 17", list.Tail.Value == 17);
            Assert("Deleting back node: total node count equals 2", list.Count == 2);

            list.Remove(42);
            Assert("Deleting interior node: total node count equals 1", list.Count == 1);

            bool removed = list.Remove(63);
            Assert("Deleting absent value: operation reports failure", !removed);

            list.AddLast(74);
            list.AddLast(31);
            list.RemoveFirst();
            Assert("Dropping the head: new head is 74", list.Head.Value == 74);
            list.RemoveLast();
            Assert("Dropping the tail: new tail is 74", list.Tail.Value == 74);

            list.Clear();
            Assert("Wiping the list: node count drops to zero", list.Count == 0);
            Assert("Wiping the list: head pointer is null", list.Head == null);
            Assert("Wiping the list: tail pointer is null", list.Tail == null);

            list.AddLast(58);
            list.AddLast(93);
            Assert("Peeking the front: returns 58", list.First() == 58);
            Assert("Peeking the back: returns 93", list.Last() == 93);

            list.Clear();
            AssertThrows<InvalidOperationException>("Peeking front of empty list: exception expected", () => list.First());
            AssertThrows<InvalidOperationException>("Peeking back of empty list: exception expected", () => list.Last());

            list.AddLast(11);
            list.AddLast(47);
            list.AddLast(66);
            int[] expected = { 11, 47, 66 };
            Assert("Iterating: nodes appear in insertion order", list.SequenceEqual(expected));

            var arr = new int[3];
            list.CopyTo(arr, 0);
            Assert("Copying to array: values match the list contents", arr.SequenceEqual(expected));
        }
        #endregion

        #region LinkedList Stack Tests
        static void TestLinkedListStack()
        {
            Section("MyStack (LinkedList-based)");

            var stack = new MyStack<int>();

            Assert("Newly created stack: size is zero", stack.Count == 0);
            Assert("Newly created stack: reports as empty", stack.IsEmpty);

            stack.Push(38);
            stack.Push(76);
            stack.Push(54);
            Assert("After three pushes: size is three", stack.Count == 3);
            Assert("After three pushes: no longer empty", !stack.IsEmpty);

            Assert("Inspecting top: sees 54 without removing it", stack.Peek() == 54);
            Assert("Inspecting top: size remains three", stack.Count == 3);

            Assert("Popping: retrieves 54 first", stack.Pop() == 54);
            Assert("Popping: retrieves 76 next", stack.Pop() == 76);
            Assert("Popping: size is now one", stack.Count == 1);

            stack.Pop();
            Assert("Popping last element: size reaches zero", stack.Count == 0);

            AssertThrows<InvalidOperationException>("Popping from empty stack: exception expected", () => stack.Pop());
            AssertThrows<InvalidOperationException>("Inspecting empty stack top: exception expected", () => stack.Peek());
        }
        #endregion

        #region Array Stack Tests
        static void TestArrayStack()
        {
            Section("MyArrayStack (Array-based)");

            var stack = new ArrayStack<int>();

            stack.Push(29);
            stack.Push(61);
            stack.Push(83);
            Assert("Inspecting top: sees 83", stack.Peek() == 83);

            Assert("Popping: retrieves 83", stack.Pop() == 83);
            Assert("Popping: retrieves 61", stack.Pop() == 61);
            Assert("Popping: retrieves 29", stack.Pop() == 29);

            AssertThrows<InvalidOperationException>("Popping from empty stack: exception expected", () => stack.Pop());
            AssertThrows<InvalidOperationException>("Inspecting empty stack top: exception expected", () => stack.Peek());

            for (int i = 0; i < 20; i++)
                stack.Push(i);
            Assert("Pushing 20 items: internal resize handled correctly", stack.Peek() == 19);

            for (int i = 19; i >= 0; i--)
                Assert($"Popping after resize: value {i} comes out in order", stack.Pop() == i);
        }
        #endregion

        #region Queue Tests
        static void TestQueue()
        {
            Section("MyQueue");

            var queue = new MyQueue<int>();

            Assert("Newly created queue: size is zero", queue.Count == 0);

            queue.Enqueue(48);
            queue.Enqueue(72);
            queue.Enqueue(15);
            Assert("After three enqueues: size is three", queue.Count == 3);

            Assert("Inspecting front: sees 48 without removing it", queue.Peek() == 48);
            Assert("Inspecting front: size remains three", queue.Count == 3);

            Assert("Dequeuing: retrieves 48 first", queue.Dequeue() == 48);
            Assert("Dequeuing: retrieves 72 next", queue.Dequeue() == 72);
            Assert("Dequeuing: size is now one", queue.Count == 1);

            queue.Dequeue();
            AssertThrows<InvalidOperationException>("Dequeuing from empty queue: exception expected", () => queue.Dequeue());
            AssertThrows<InvalidOperationException>("Inspecting empty queue front: exception expected", () => queue.Peek());

            queue.Enqueue(53);
            queue.Enqueue(27);
            queue.Enqueue(89);
            int[] expected = { 53, 27, 89 };
            Assert("Iterating: elements appear in FIFO order", queue.SequenceEqual(expected));
        }
        #endregion

        #region Binary Tree Tests
        static void TestBinaryTree()
        {
            Section("MyBinaryTree");

            var tree = new MyBinaryTree<int>();

            Assert("Newly created tree: size is zero", tree.Count == 0);

            tree.Add(50);
            tree.Add(30);
            tree.Add(70);
            tree.Add(20);
            tree.Add(40);
            tree.Add(60);
            tree.Add(80);
            Assert("After seven insertions: size is seven", tree.Count == 7);

            Assert("Searching: 50 is found in the tree", tree.Contains(50));
            Assert("Searching: 20 is found in the tree", tree.Contains(20));
            Assert("Searching: 80 is found in the tree", tree.Contains(80));
            Assert("Searching: 99 is not present in the tree", !tree.Contains(99));

            int[] expected = { 20, 30, 40, 50, 60, 70, 80 };
            Assert("In-order traversal: yields values in ascending order", tree.SequenceEqual(expected));

            tree.Remove(20);
            Assert("Removing a leaf node: size drops to six", tree.Count == 6);
            Assert("Removing a leaf node: 20 is no longer found", !tree.Contains(20));

            tree.Remove(30);
            Assert("Removing a single-child node: size drops to five", tree.Count == 5);
            Assert("Removing a single-child node: 30 is no longer found", !tree.Contains(30));
            Assert("Removing a single-child node: child 40 remains in tree", tree.Contains(40));

            tree.Remove(70);
            Assert("Removing a two-child node: size drops to four", tree.Count == 4);
            Assert("Removing a two-child node: 70 is no longer found", !tree.Contains(70));
            Assert("Removing a two-child node: both children 60 and 80 remain", tree.Contains(60) && tree.Contains(80));

            tree.Remove(50);
            Assert("Removing the root: size drops to three", tree.Count == 3);
            Assert("Removing the root: remaining nodes still in BST order", tree.SequenceEqual(new[] { 40, 60, 80 }));

            bool removed = tree.Remove(99);
            Assert("Removing an absent value: operation reports failure", !removed);
        }
        #endregion
    }
}