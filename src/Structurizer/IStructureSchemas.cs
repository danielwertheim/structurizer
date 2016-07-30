using System;
using System.Collections.Generic;

namespace Structurizer
{
    public interface IStructureSchemas
    {
        IStructureTypeFactory StructureTypeFactory { get; set; }
        IStructureSchemaFactory StructureSchemaFactory { get; set; }

        void Clear();
        IEnumerable<KeyValuePair<Type, IStructureSchema>> GetRegistrations();
        IStructureSchema GetSchema<T>() where T : class;
        IStructureSchema GetSchema(Type type);
        IEnumerable<IStructureSchema> GetSchemas();
        void RemoveSchema(Type type);
    }
}