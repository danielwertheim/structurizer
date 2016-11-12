using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Structurizer
{
    public class StructureIndexesFactory : IStructureIndexesFactory
    {
        public IStructureIndex[] CreateIndexes<T>(IStructureSchema structureSchema, T item) where T : class
        {
            var indexes = new IEnumerable<IStructureIndex>[structureSchema.IndexAccessors.Count];

#if DEBUG
            Parallel.For(0, indexes.Length, new ParallelOptions { MaxDegreeOfParallelism = 1 }, c =>
#else
            Parallel.For(0, indexes.Length, c =>
#endif
            {
                var indexAccessor = structureSchema.IndexAccessors[c];
                var values = indexAccessor.GetValues(item);

                var valuesExists = values != null && values.Count > 0;
                if (!valuesExists)
                    return;

                var isCollectionOfValues = indexAccessor.IsEnumerable || indexAccessor.IsElement || values.Count > 1;
                if (!isCollectionOfValues)
                    indexes[c] = new[]
                    {
                        new StructureIndex(values[0].Path, values[0].Value, indexAccessor.DataType, indexAccessor.DataTypeCode)
                    };
                else
                {
                    var subIndexes = new IStructureIndex[values.Count];
#if DEBUG
                    Parallel.For(0, subIndexes.Length, new ParallelOptions { MaxDegreeOfParallelism = 1 }, subC =>
#else
                    Parallel.For(0, subIndexes.Length, subC =>
#endif
                    {
                        if (values[subC] != null)
                            subIndexes[subC] = new StructureIndex(
                                values[subC].Path,
                                values[subC].Value,
                                indexAccessor.DataType,
                                indexAccessor.DataTypeCode);
                    });
                    indexes[c] = subIndexes;
                }
            });

            return indexes
                .Where(i => i != null)
                .SelectMany(i => i)
                .Where(i => i != null)
                .ToArray();
        }
    }
}