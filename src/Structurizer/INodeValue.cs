namespace Structurizer
{
    public interface INodeValue
    {
        string Path { get; }
        object Value { get; }
    }
}