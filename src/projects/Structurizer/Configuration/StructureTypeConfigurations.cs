using System;
using System.Collections;
using System.Collections.Generic;
using EnsureThat;

namespace Structurizer.Configuration
{
    public class StructureTypeConfigurations : IStructureTypeConfigurations
    {
        private readonly Dictionary<Type, IStructureTypeConfig> _configurations;

        public StructureTypeConfigurations()
        {
            _configurations = new Dictionary<Type, IStructureTypeConfig>();
        }

        IEnumerator<IStructureTypeConfig> IEnumerable<IStructureTypeConfig>.GetEnumerator() => _configurations.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _configurations.Values.GetEnumerator();

        public IStructureTypeConfig GetConfiguration<T>() where T : class => GetConfiguration(typeof(T));

        public IStructureTypeConfig GetConfiguration(Type type)
        {
            IStructureTypeConfig config;

            return _configurations.TryGetValue(type, out config) ? config : null;
        }

        public IStructureTypeConfig Register(Type structureType, Action<IStructureTypeConfigurator> config = null)
        {
            Ensure.That(structureType, nameof(structureType)).IsNotNull();

            var configurator = new StructureTypeConfigurator(structureType);
            config?.Invoke(configurator);

            var typeConfig = configurator.GenerateConfig();
            _configurations.Add(typeConfig.Type, typeConfig);

            return typeConfig;
        }

        public IStructureTypeConfig Register<T>(Action<IStructureTypeConfigurator<T>> config = null) where T : class
        {
            var configurator = new StructureTypeConfigurator<T>(typeof(T));
            config?.Invoke(configurator);

            var typeConfig = configurator.GenerateConfig();
            _configurations.Add(typeConfig.Type, typeConfig);

            return typeConfig;
        }
    }
}