using System;

namespace Structurizer
{
    public interface IStructureTypeFactory
    {
        IStructureTypeReflecter Reflecter { get; }
        IStructureTypeConfigurations Configurations { get; }

        IStructureType CreateFor<T>() where T : class;
        IStructureType CreateFor(Type structureType);
    }
}