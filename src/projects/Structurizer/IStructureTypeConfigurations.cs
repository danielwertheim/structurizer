using System;
using System.Collections.Generic;

namespace Structurizer
{
    public interface IStructureTypeConfigurations : IEnumerable<IStructureTypeConfig>
    {
        IStructureTypeConfig Register(Type structureType, Action<IStructureTypeConfigurator> config = null);
        IStructureTypeConfig Register<T>(Action<IStructureTypeConfigurator<T>> config = null) where T : class;

        IStructureTypeConfig GetConfiguration(Type type);
        IStructureTypeConfig GetConfiguration<T>() where T : class;
    }
}