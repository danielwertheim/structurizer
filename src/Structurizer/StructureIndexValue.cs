namespace Structurizer
{
    internal class StructureIndexValue : IStructureIndexValue
    {
        public string Path { get; }
        public object Value { get; }

        internal StructureIndexValue(string path, object value)
        {
            Path = path;
            Value = value;
        }
    }
}