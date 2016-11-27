using System.Collections.Generic;

namespace Structurizer
{
    public class StructureIndexesFactory : IStructureIndexesFactory
    {
        public IList<IStructureIndex> CreateIndexes<T>(IStructureSchema structureSchema, T item) where T : class
        {
            var result = new List<IStructureIndex>();

            for (var i = 0; i < structureSchema.IndexAccessors.Count; i++)
            {
                var indexAccessor = structureSchema.IndexAccessors[i];
                var values = indexAccessor.GetValues(item);

                var valuesExists = values != null && values.Count > 0;
                if (!valuesExists)
                    continue;

                var isCollectionOfValues = indexAccessor.IsEnumerable || indexAccessor.IsElement || values.Count > 1;
                if (!isCollectionOfValues)
                {
                    if (values[0].Value == null)
                        continue;

                    result.Add(new StructureIndex(indexAccessor.Path, values[0].Path, values[0].Value, indexAccessor.DataType, indexAccessor.DataTypeCode));
                }
                else
                {
                    for (var subC = 0; subC < values.Count; subC++)
                    {
                        if (values[subC] == null)
                            continue;

                        if (values[subC].Value == null)
                            continue;

                        result.Add(new StructureIndex(
                            indexAccessor.Path,
                            values[subC].Path,
                            values[subC].Value,
                            indexAccessor.DataType,
                            indexAccessor.DataTypeCode));
                    }
                }
            }

            return result;
        }
    }
}