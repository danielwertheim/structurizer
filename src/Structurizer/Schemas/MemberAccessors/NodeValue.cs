namespace Structurizer.Schemas.MemberAccessors
{
    internal class NodeValue : INodeValue
    {
        public string Path { get; }
        public object Value { get; }

        public NodeValue(string path, object value)
        {
            Path = path;
            Value = value;
        }
    }
}