using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using EnsureThat;
using Structurizer.Configuration;

namespace Structurizer
{
    public class StructureBuilder : IStructureBuilder
    {
        private readonly ConcurrentDictionary<Type, IStructureSchema> _schemas = new ConcurrentDictionary<Type, IStructureSchema>();

        protected IStructureTypeConfigurations TypeConfigurations { get; }
        protected IStructureTypeFactory TypeFactory { get; }
        protected IStructureSchemaFactory SchemaFactory { get; }
        protected IStructureIndexesFactory IndexesFactory { get; }

        private StructureBuilder(
            IStructureTypeConfigurations typeConfigurations,
            IStructureTypeFactory structureTypeFactory,
            IStructureSchemaFactory schemaFactory,
            IStructureIndexesFactory indexesFactory)
        {
            Ensure.That(typeConfigurations, nameof(typeConfigurations)).IsNotNull();
            Ensure.That(structureTypeFactory, nameof(structureTypeFactory)).IsNotNull();
            Ensure.That(schemaFactory, nameof(schemaFactory)).IsNotNull();
            Ensure.That(indexesFactory, nameof(indexesFactory)).IsNotNull();

            TypeConfigurations = typeConfigurations;
            TypeFactory = structureTypeFactory;
            SchemaFactory = schemaFactory;
            IndexesFactory = indexesFactory;
        }

        public static IStructureBuilder Create(Action<IStructureTypeConfigurations> config)
        {
            Ensure.That(config, nameof(config)).IsNotNull();

            var configs = new StructureTypeConfigurations();

            config(configs);

            return Create(configs);
        }

        public static IStructureBuilder Create(IStructureTypeConfigurations config)
        {
            Ensure.That(config, nameof(config)).IsNotNull();

            return new StructureBuilder(
                config,
                new StructureTypeFactory(),
                new StructureSchemaFactory(),
                new StructureIndexesFactory());
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

        private IStructureSchema GetSchema(Type type) => _schemas.GetOrAdd(type, CreateSchema);

        private IStructureSchema CreateSchema(Type type)
        {
            var typeConfig = TypeConfigurations.GetConfiguration(type);
            var structureType = TypeFactory.CreateFor(typeConfig);

            return SchemaFactory.CreateSchema(structureType);
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