using System.Collections.Generic;

namespace Structurizer
{
    public interface IStructureIndexesFactory
    {
        IList<IStructureIndex> CreateIndexes<T>(IStructureSchema structureSchema, T item) where T : class;
    }
}