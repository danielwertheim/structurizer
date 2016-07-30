using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Structurizer.Configuration;

namespace Structurizer.UnitTests.Configuration
{
    [TestFixture]
    public class StructureTypeConfigurationsTests : UnitTestBase
    {
        [Test]
        public void Register_Should_add_config_When_no_registration_exists()
        {
            var configs = new StructureTypeConfigurations();

            configs.Register(typeof(Dummy));

            Assert.AreEqual(1, configs.Count());
        }

        [Test]
        public void Register_with_empty_config_Should_add_config_When_no_registration_exists()
        {
            var configs = new StructureTypeConfigurations();

            configs.Register(typeof(Dummy), cfg => { });

            Assert.AreEqual(1, configs.Count());
        }

        [Test]
        public void Register_generic_Should_add_config_When_no_registration_exists()
        {
            var configs = new StructureTypeConfigurations();

            configs.Register<Dummy>();

            Assert.AreEqual(1, configs.Count());
        }

        [Test]
        public void Register_generic_with_empty_config_Should_add_config_When_no_registration_exists()
        {
            var configs = new StructureTypeConfigurations();

            configs.Register<Dummy>(cfg => { });

            Assert.AreEqual(1, configs.Count());
        }

        [Test]
        public void Register_Should_add_to_existing_config_When_called_twice()
        {
            var configs = new StructureTypeConfigurations();
            IStructureTypeConfig config1 = null;
            IStructureTypeConfig config2 = null;

            configs.Register(typeof(Dummy), cfg => { config1 = cfg.Config; });
            configs.Register(typeof(Dummy), cfg => { config2 = cfg.Config; });

            Assert.AreSame(config1, config2);
            Assert.AreEqual(1, configs.Count());
        }

        [Test]
        public void Register_generic_Should_add_to_existing_config_When_called_twice()
        {
            var configs = new StructureTypeConfigurations();
            IStructureTypeConfig config1 = null;
            IStructureTypeConfig config2 = null;

            configs.Register<Dummy>(cfg => { config1 = cfg.Config; });
            configs.Register<Dummy>(cfg => { config2 = cfg.Config; });

            Assert.AreSame(config1, config2);
            Assert.AreEqual(1, configs.Count());
        }

        [Test]
        public void GetConfigurations_Should_return_null_When_no_config_registration_exists()
        {
            var configs = new StructureTypeConfigurations();

            var config = configs.GetConfiguration(typeof(Dummy));

            config.Should().BeNull();
        }

        [Test]
        public void GetConfigurations_Should_return_config_When_registrated_via_generic_version()
        {
            var configs = new StructureTypeConfigurations();
            configs.Register<Dummy>(cfg => { });

            var config = configs.GetConfiguration(typeof(Dummy));

            Assert.IsNotNull(config);
            Assert.AreEqual(typeof(Dummy), config.Type);
        }

        [Test]
        public void GetConfigurations_Should_return_config_When_registrated_via_non_generic_version()
        {
            var configs = new StructureTypeConfigurations();
            configs.Register(typeof(Dummy), cfg => { });

            var config = configs.GetConfiguration(typeof(Dummy));

            Assert.IsNotNull(config);
            Assert.AreEqual(typeof(Dummy), config.Type);
        }

        private class Dummy { }
    }
}