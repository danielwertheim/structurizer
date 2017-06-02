using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using EnsureThat;

namespace Structurizer.Configuration
{
    public class StructureTypeConfigurations : IStructureTypeConfigurations
    {
        private readonly ConcurrentDictionary<Type, IStructureTypeConfig> _configurations;

        public StructureTypeConfigurations()
        {
            _configurations = new ConcurrentDictionary<Type, IStructureTypeConfig>();
        }

        IEnumerator<IStructureTypeConfig> IEnumerable<IStructureTypeConfig>.GetEnumerator() => _configurations.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _configurations.Values.GetEnumerator();

        public IStructureTypeConfig GetConfiguration<T>() where T : class => GetConfiguration(typeof(T));

        public IStructureTypeConfig GetConfiguration(Type type) => _configurations.TryGetValue(type, out IStructureTypeConfig config)
            ? config
            : null;

        public IStructureTypeConfig Register(Type structureType, Action<IStructureTypeConfigurator> configurator = null)
        {
            Ensure.That(structureType, nameof(structureType)).IsNotNull();

            return _configurations.AddOrUpdate(
                structureType,
                t => CreateStructureTypeConfig(t, configurator),
                (t, existing) => CreateStructureTypeConfig(t, configurator));
        }

        private static IStructureTypeConfig CreateStructureTypeConfig(Type structureType, Action<IStructureTypeConfigurator> config = null)
        {
            var configurator = new StructureTypeConfigurator(structureType);
            config?.Invoke(configurator);

            return configurator.GenerateConfig();
        }

        public IStructureTypeConfig Register<T>(Action<IStructureTypeConfigurator<T>> configurator = null) where T : class
            => _configurations.AddOrUpdate(
                typeof(T),
                t => CreateStructureTypeConfig(t, configurator),
                (t, existing) => CreateStructureTypeConfig(t, configurator));

        private static IStructureTypeConfig CreateStructureTypeConfig<T>(Type structureType, Action<IStructureTypeConfigurator<T>> config = null) where T : class
        {
            var configurator = new StructureTypeConfigurator<T>(structureType);
            config?.Invoke(configurator);

            return configurator.GenerateConfig();
        }
    }
}