using Structurizer.Schemas;

namespace Structurizer
{
    public interface IStructureIndexesFactory
    {
        IStructureIndex[] CreateIndexes<T>(IStructureSchema structureSchema, T item) where T : class;
    }
}