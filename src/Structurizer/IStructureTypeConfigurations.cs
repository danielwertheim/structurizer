using System;
using System.Collections.Generic;

namespace Structurizer
{
    public interface IStructureTypeConfigurations : IEnumerable<IStructureTypeConfig>
    {
        IStructureTypeConfig Register(Type type);
        IStructureTypeConfig Register(Type type, Action<IStructureTypeConfigurator> configure);
        IStructureTypeConfig Register<T>() where T : class;
        IStructureTypeConfig Register<T>(Action<IStructureTypeConfigurator<T>> configure) where T : class;

        IStructureTypeConfig GetConfiguration(Type type);
        IStructureTypeConfig GetConfiguration<T>() where T : class;
    }
}