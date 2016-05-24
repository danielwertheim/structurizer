using System;
using Structurizer.Schemas.Configuration;

namespace Structurizer.Schemas
{
    public interface IStructureTypeFactory
    {
        Func<IStructureTypeConfig, IStructureTypeReflecter> ReflecterFn { get; set; }
        IStructureTypeConfigurations Configurations { get; set; }

        IStructureType CreateFor<T>() where T : class;
        IStructureType CreateFor(Type structureType);
    }
}