using System.Threading.Tasks;

namespace Structurizer
{
    public class StructureBuilder : IStructureBuilder
    {
        public IStructureIndexesFactory IndexesFactory { get; set; }
        public IStructureSchemaFactory SchemaFactory { get; set; } //TODO: Use this, and adapt StructureBuilder to only take item as argument

        public StructureBuilder()
        {
            IndexesFactory = new StructureIndexesFactory();
        }

        public IStructure CreateStructure<T>(T item, IStructureSchema structureSchema) where T : class
        {
            return new Structure(
                structureSchema.Name,
                IndexesFactory.CreateIndexes(structureSchema, item));
        }

        public IStructure[] CreateStructures<T>(T[] items, IStructureSchema structureSchema) where T : class
        {
            return items.Length < 100
                ? CreateStructuresInSerial(items, structureSchema)
                : CreateStructuresInParallel(items, structureSchema);
        }

        private IStructure[] CreateStructuresInParallel<T>(T[] items, IStructureSchema structureSchema) where T : class
        {
            var structures = new IStructure[items.Length];

            Parallel.For(0, items.Length, i =>
            {
                var itm = items[i];

                structures[i] = new Structure(
                    structureSchema.Name,
                    IndexesFactory.CreateIndexes(structureSchema, itm));
            });

            return structures;
        }

        private IStructure[] CreateStructuresInSerial<T>(T[] items, IStructureSchema structureSchema) where T : class
        {
            var structures = new IStructure[items.Length];

            for (var i = 0; i < structures.Length; i++)
            {
                var itm = items[i];

                structures[i] = new Structure(
                    structureSchema.Name,
                    IndexesFactory.CreateIndexes(structureSchema, itm));
            }

            return structures;
        }
    }
}