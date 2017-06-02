using System;
using System.Collections.Concurrent;
using System.Linq;
using EnsureThat;
using Structurizer.Configuration;

namespace Structurizer
{
    /// <summary>
    /// This builder adopts as time goes in the sense that if you
    /// ask it to create a structure for a type it does not know of,
    /// it creates the schemas etc and caches them. You can reconfigure
    /// it at anytime using the <see cref="Configure{T}"/> and the
    /// <see cref="ConfigureUsingTemplate{T}"/> methods.
    /// </summary>
    public class FlexibleStructureBuilder : IStructureBuilder
    {
        private readonly IStructureTypeConfigurations _typeConfigurations;
        private readonly IStructureTypeFactory _typeFactory;
        private readonly IStructureSchemaFactory _schemaFactory;
        private readonly IStructureIndexesFactory _indexesFactory;
        private readonly ConcurrentDictionary<Type, IStructureSchema> _schemas;

        public FlexibleStructureBuilder()
        {
            _typeConfigurations = new StructureTypeConfigurations();
            _typeFactory = new StructureTypeFactory();
            _schemaFactory = new StructureSchemaFactory();
            _indexesFactory = new StructureIndexesFactory();
            _schemas = new ConcurrentDictionary<Type, IStructureSchema>();
        }

        public void Configure(Type structureType, Action<IStructureTypeConfigurator> configurator = null)
        {
            EnsureArg.IsNotNull(structureType, nameof(structureType));

            var typeConfig = _typeConfigurations.Register(structureType, configurator);

            _schemas.AddOrUpdate(
                structureType,
                t => CreateSchema(t, typeConfig),
                (type, schema) => CreateSchema(type, typeConfig));
        }

        public void Configure<T>(Action<IStructureTypeConfigurator<T>> configurator = null) where T : class
        {
            var typeConfig = _typeConfigurations.Register(configurator);

            _schemas.AddOrUpdate(
                typeof(T),
                t => CreateSchema(t, typeConfig),
                (type, schema) => CreateSchema(type, typeConfig));
        }

        public void ConfigureUsingTemplate<T>(T template, Action<IStructureTypeConfigurator<T>> configurator = null) where T : class
            => Configure(configurator);

        public IStructure CreateStructure<T>(T item) where T : class
        {
            EnsureArg.IsNotNull(item, nameof(item));

            var schema = GetSchema(typeof(T));

            return new Structure(schema.Name, _indexesFactory.CreateIndexes(schema, item));
        }

        public IStructure[] CreateStructures<T>(T[] items) where T : class
        {
            EnsureArg.HasItems(items, nameof(items));

            return items.Select(CreateStructure).ToArray();
        }

        private IStructureSchema GetSchema(Type type)
            => _schemas.GetOrAdd(type, t => CreateSchema(t));

        private IStructureSchema CreateSchema(Type type, IStructureTypeConfig typeConfig = null)
        {
            typeConfig = typeConfig ?? _typeConfigurations.GetConfiguration(type) ?? _typeConfigurations.Register(type);
            var structureType = _typeFactory.CreateFor(typeConfig);

            return _schemaFactory.CreateSchema(structureType);
        }
    }
}