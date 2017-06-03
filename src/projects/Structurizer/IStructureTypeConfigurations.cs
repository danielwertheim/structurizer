using System;
using System.Collections.Generic;

namespace Structurizer
{
    public interface IStructureTypeConfigurations : IEnumerable<IStructureTypeConfig>
    {
        /// <summary>
        /// Registreres and configures a certain type.
        /// </summary>
        /// <param name="structureType"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        IStructureTypeConfig Register(Type structureType, Action<IStructureTypeConfigurator> configurator = null);

        /// <summary>
        /// Registreres and configures a certain type.
        /// </summary>
        /// <typeparam name="T">The type to configure</typeparam>
        /// <param name="configurator"></param>
        /// <returns></returns>
        IStructureTypeConfig Register<T>(Action<IStructureTypeConfigurator<T>> configurator = null) where T : class;

        /// <summary>
        /// Returns any registrered configuration for the defined type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IStructureTypeConfig GetConfiguration(Type type);

        /// <summary>
        /// Returns any regisrered configuration for the generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IStructureTypeConfig GetConfiguration<T>() where T : class;
    }
}