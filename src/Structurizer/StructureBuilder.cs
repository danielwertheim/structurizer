using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnsureThat;
using Structurizer.Configuration;

namespace Structurizer
{
    public class StructureBuilder : IStructureBuilder
    {
        protected IDictionary<Type, IStructureSchema> Schemas { get; }
        protected IStructureIndexesFactory IndexesFactory { get; }

        private StructureBuilder(
            IDictionary<Type, IStructureSchema> schemas,
            IStructureIndexesFactory indexesFactory = null)
        {
            Ensure.That(schemas, nameof(schemas)).IsNotNull().HasItems();

            Schemas = schemas;
            IndexesFactory = indexesFactory ?? new StructureIndexesFactory();
        }

        public static IStructureBuilder Create(Action<IStructureTypeConfigurations> config)
        {
            Ensure.That(config, nameof(config)).IsNotNull();

            var configs = new StructureTypeConfigurations();

            config(configs);

            return Create(configs);
        }

        public static IStructureBuilder Create(IStructureTypeConfigurations typeConfigs)
        {
            Ensure.That(typeConfigs, nameof(typeConfigs)).IsNotNull();

            var structureTypeFactory = new StructureTypeFactory();
            var schemaFactory = new StructureSchemaFactory();

            var schemas = typeConfigs
                .Select(tc => structureTypeFactory.CreateFor(tc))
                .Select(st => schemaFactory.CreateSchema(st))
                .ToDictionary(s => s.StructureType.Type);

            return new StructureBuilder(schemas);
        }

        public IStructure CreateStructure<T>(T item) where T : class
        {
            var schema = GetSchema(typeof(T));

            return new Structure(
                schema.Name,
                IndexesFactory.CreateIndexes(schema, item));
        }

        public IStructure[] CreateStructures<T>(T[] items) where T : class
        {
            var schema = GetSchema(typeof(T));

            return items.Length < 100
                ? CreateStructuresInSerial(items, schema)
                : CreateStructuresInParallel(items, schema);
        }

        private IStructureSchema GetSchema(Type type) => Schemas[type];

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