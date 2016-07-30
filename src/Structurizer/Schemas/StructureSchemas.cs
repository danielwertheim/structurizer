using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;

namespace Structurizer.Schemas
{
    //TODO: Morph into CachedStructureSchemaFactory
    public class StructureSchemas
    {
        private readonly ConcurrentDictionary<Type, IStructureSchema> _schemas;
        private readonly Func<Type, IStructureSchema> _schemaFactoryFn;

        protected IStructureTypeFactory StructureTypeFactory { get; }
        protected IStructureSchemaFactory StructureSchemaFactory { get; }

        public StructureSchemas(IStructureTypeFactory structureTypeFactory, IStructureSchemaFactory structureSchemaFactory)
        {
            Ensure.That(structureTypeFactory, "structureTypeFactory").IsNotNull();
            Ensure.That(structureSchemaFactory, "StructureSchemaFactory").IsNotNull();

            StructureTypeFactory = structureTypeFactory;
            StructureSchemaFactory = structureSchemaFactory;
            _schemas = new ConcurrentDictionary<Type, IStructureSchema>();

            _schemaFactoryFn = t => StructureSchemaFactory.CreateSchema(StructureTypeFactory.CreateFor(t));
        }

        public IStructureSchema GetSchema<T>() where T : class
        {
            return GetSchema(typeof(T));
        }

        public IStructureSchema GetSchema(Type type)
        {
            Ensure.That(type, "type").IsNotNull();

            return _schemas.GetOrAdd(type, _schemaFactoryFn);
        }

        public IEnumerable<IStructureSchema> GetSchemas()
        {
            return _schemas.Values;
        }

        public void RemoveSchema(Type type)
        {
            Ensure.That(type, "type").IsNotNull();

            IStructureSchema tmp;

            _schemas.TryRemove(type, out tmp);
        }

        public IEnumerable<KeyValuePair<Type, IStructureSchema>> GetRegistrations()
        {
            return _schemas.ToList();
        }

        public void Clear()
        {
            _schemas.Clear();
        }
    }
}