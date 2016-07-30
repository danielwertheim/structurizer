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

        public IStructureTypeConfig Register(Type type)
        {
            Ensure.That(type, nameof(type)).IsNotNull();

            var config = GetConfiguration(type) ?? new StructureTypeConfig(type);

            _configurations[config.Type] = config;

            return config;
        }

        public IStructureTypeConfig Register(Type type, Action<IStructureTypeConfigurator> configure)
        {
            Ensure.That(type, nameof(type)).IsNotNull();
            Ensure.That(configure, nameof(configure)).IsNotNull();

            var config = Register(type);
            var configurator = new StructureTypeConfigurator(config);
            configure(configurator);

            return config;
        }

        public IStructureTypeConfig Register<T>() where T : class => Register(typeof(T));

        public IStructureTypeConfig Register<T>(Action<IStructureTypeConfigurator<T>> configure) where T : class
        {
            Ensure.That(configure, nameof(configure)).IsNotNull();

            var config = Register<T>();
            var configurator = new StructureTypeConfigurator<T>(config);
            configure(configurator);

            return config;
        }
    }
}