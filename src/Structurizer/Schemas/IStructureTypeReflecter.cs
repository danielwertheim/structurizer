using System;
using System.Collections.Generic;

namespace Structurizer.Schemas
{
    /// <summary>
    /// Responsible for identifying the Properties that should be used as
    /// indexes for a certain structure type.
    /// </summary>
    public interface IStructureTypeReflecter
    {
        IStructurePropertyFactory PropertyFactory { set; }

        IStructureProperty[] GetIndexableProperties(Type structureType);
        IStructureProperty[] GetIndexablePropertiesExcept(Type structureType, ICollection<string> nonIndexablePaths);
        IStructureProperty[] GetSpecificIndexableProperties(Type structureType, ICollection<string> indexablePaths);
    }
}