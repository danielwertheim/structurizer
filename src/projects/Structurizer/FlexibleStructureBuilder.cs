using System;
using System.Collections.Concurrent;
using System.Linq;
using EnsureThat;
using Structurizer.Configuration;

namespace Structurizer
{
    public class FlexibleStructureBuilder : IStructureBuilder
    {
        private readonly StructureTypeConfigurations _typeConfigurations;
        private readonly StructureTypeFactory _typeFactory;
        private readonly StructureSchemaFactory _schemaFactory;
        private readonly StructureIndexesFactory _indexesFactory;
        private readonly ConcurrentDictionary<Type, IStructureSchema> _schemas;

        public FlexibleStructureBuilder()
        {
            _typeConfigurations = new StructureTypeConfigurations();
            _typeFactory = new StructureTypeFactory();
            _schemaFactory = new StructureSchemaFactory();
            _indexesFactory = new StructureIndexesFactory();
            _schemas = new ConcurrentDictionary<Type, IStructureSchema>();
        }

        public IStructure CreateStructure<T>(T item) where T : class
        {
            EnsureArg.IsNotNull(item, nameof(item));

            var schema = GetSchema(typeof(T));

            return new Structure(schema.Name, _indexesFactory.CreateIndexes(schema, item));
        }

        public IStructure[] CreateStructures<T>(T[] items) where T : class
        {
            EnsureArg.HasItems(items, nameof(items));

            return items.Select(CreateStructure).ToArray<IStructure>();
        }

        private IStructureSchema GetSchema(Type type)
        {
            return _schemas.GetOrAdd(type, CreateSchema);
        }

        private IStructureSchema CreateSchema(Type type)
        {
            var typeConfig = _typeConfigurations.GetConfiguration(type) ?? _typeConfigurations.Register(type);
            var structureType = _typeFactory.CreateFor(typeConfig);

            return _schemaFactory.CreateSchema(structureType);
        }
    }
}