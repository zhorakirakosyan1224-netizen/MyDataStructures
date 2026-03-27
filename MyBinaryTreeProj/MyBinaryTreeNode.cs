namespace MyBinaryTreeProj;

public class MyBinaryTreeNode<T> where T : IComparable<T>
{
    public T Value { get; private set; }
    public MyBinaryTreeNode<T> Left { get; set; }
    public MyBinaryTreeNode<T> Right { get; set; }

    public MyBinaryTreeNode(T value)
    {
        Value = value;
    }
}