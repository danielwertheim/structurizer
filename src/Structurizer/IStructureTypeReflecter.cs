using System;
using System.Collections.Generic;

namespace Structurizer
{
    /// <summary>
    /// Responsible for identifying the Properties that should be used as
    /// indexes for a certain structure type.
    /// </summary>
    public interface IStructureTypeReflecter
    {
        IStructureProperty[] GetIndexableProperties(Type structureType);
        IStructureProperty[] GetIndexablePropertiesExcept(Type structureType, IList<string> memberPaths);
        IStructureProperty[] GetSpecificIndexableProperties(Type structureType, IList<string> memberPaths);
    }
}