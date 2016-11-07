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

            Parallel.For(0, indexes.Length, new ParallelOptions { MaxDegreeOfParallelism = 1 }, c =>
              {
                  var indexAccessor = structureSchema.IndexAccessors[c];
                  var values = indexAccessor.GetValues(item);
                  var valuesExists = values != null && values.Count > 0;
                  var isCollectionOfValues = indexAccessor.IsEnumerable || indexAccessor.IsElement || (values != null && values.Count > 1);

                  if (!valuesExists)
                      return;

                  if (!isCollectionOfValues)
                      indexes[c] = new[]
                      {
                        new StructureIndex(indexAccessor.Path, values[0], indexAccessor.DataType, indexAccessor.DataTypeCode)
                      };
                  else
                  {
                      var subIndexes = new IStructureIndex[values.Count];
                      Parallel.For(0, subIndexes.Length, subC =>
                      {
                          if (values[subC] != null)
                              subIndexes[subC] = new StructureIndex(
                                  indexAccessor.Path,
                                  values[subC],
                                  indexAccessor.DataType,
                                  indexAccessor.DataTypeCode);
                      });
                      indexes[c] = subIndexes;
                  }
              });

            return indexes.Where(i => i != null).SelectMany(i => i).Where(i => i != null).ToArray();
        }
    }
}