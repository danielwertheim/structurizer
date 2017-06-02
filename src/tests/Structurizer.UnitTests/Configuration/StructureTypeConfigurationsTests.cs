using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structurizer.Configuration;

namespace Structurizer.UnitTests.Configuration
{
    [TestClass]
    public class StructureTypeConfigurationsTests : UnitTests
    {
        [TestMethod]
        public void Register_with_null_config_Should_add_config_with_exclusive_mode()
        {
            var configs = new StructureTypeConfigurations();

            configs.Register(typeof(Dummy));

            configs.Should().HaveCount(1);
            configs.First().IndexMode.Should().Be(IndexMode.Exclusive);
        }

        [TestMethod]
        public void Register_with_empty_config_Should_add_config_with_exclusive_mode()
        {
            var configs = new StructureTypeConfigurations();

            configs.Register(typeof(Dummy), cfg => { });

            configs.Should().HaveCount(1);
            configs.First().IndexMode.Should().Be(IndexMode.Exclusive);
        }

        [TestMethod]
        public void Register_generic_with_null_config_Should_add_config_with_exclusive_mode()
        {
            var configs = new StructureTypeConfigurations();

            configs.Register<Dummy>();

            configs.Should().HaveCount(1);
            configs.First().IndexMode.Should().Be(IndexMode.Exclusive);
        }

        [TestMethod]
        public void Register_generic_with_empty_config_Should_add_config_with_exclusive_mode()
        {
            var configs = new StructureTypeConfigurations();

            configs.Register<Dummy>(cfg => { });

            configs.Should().HaveCount(1);
            configs.First().IndexMode.Should().Be(IndexMode.Exclusive);
        }

        [TestMethod]
        public void Register_Should_not_throw_When_registrering_same_type_more_than_once()
        {
            var configs = new StructureTypeConfigurations();
            configs.Register(typeof(Dummy));

            Action action = () => configs.Register(typeof(Dummy));

            action.ShouldNotThrow();
        }

        [TestMethod]
        public void Register_generic_Should_not_throw_When_registrering_same_type_more_than_once()
        {
            var configs = new StructureTypeConfigurations();
            configs.Register<Dummy>();

            Action action = () => configs.Register<Dummy>();

            action.ShouldNotThrow();
        }

        [TestMethod]
        public void GetConfigurations_Should_return_null_When_no_config_registration_exists()
        {
            var configs = new StructureTypeConfigurations();

            var config = configs.GetConfiguration(typeof(Dummy));

            config.Should().BeNull();
        }

        [TestMethod]
        public void GetConfigurations_Should_return_config_When_registrated_via_generic_version()
        {
            var configs = new StructureTypeConfigurations();
            configs.Register<Dummy>(cfg => { });

            var config = configs.GetConfiguration(typeof(Dummy));

            Assert.IsNotNull(config);
            Assert.AreEqual(typeof(Dummy), config.Type);
        }

        [TestMethod]
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