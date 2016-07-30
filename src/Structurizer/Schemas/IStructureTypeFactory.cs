using System;
using Structurizer.Schemas.Configuration;

namespace Structurizer.Schemas
{
    public interface IStructureTypeFactory
    {
        Func<IStructureTypeConfig, IStructureTypeReflecter> ReflecterFn { get; }
        IStructureTypeConfigurations Configurations { get; }

        IStructureType CreateFor<T>() where T : class;
        IStructureType CreateFor(Type structureType);
    }
}