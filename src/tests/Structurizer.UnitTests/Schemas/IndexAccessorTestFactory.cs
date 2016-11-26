using Structurizer.Schemas;

namespace Structurizer.UnitTests.Schemas
{
    internal static class IndexAccessorTestFactory
    {
        private static readonly IDataTypeConverter DataTypeConverter = new DataTypeConverter();

        internal static IIndexAccessor CreateFor(IStructureProperty property)
        {
            return new IndexAccessor(property, DataTypeConverter.Convert(property));
        }
    }
}