using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Structurizer.Configuration;

namespace Structurizer.UnitTests.Configuration
{
    [TestFixture]
    public class StructureTypeConfiguratorTests : UnitTestBase
    {
        private IStructureTypeConfig UseNonGenericConfiguratorFor<T>(Action<IStructureTypeConfigurator> configure) where T : class
        {
            var configurator = new StructureTypeConfigurator(typeof(T));

            configure(configurator);

            return configurator.GenerateConfig();
        }

        private IStructureTypeConfig UseGenericConfiguratorFor<T>(Action<IStructureTypeConfigurator<T>> configure) where T : class
        {
            var configurator = new StructureTypeConfigurator<T>(typeof(T));

            configure(configurator);

            return configurator.GenerateConfig();
        }

        [Test]
        public void NonGeneric_Should_assign_IndexMode()
        {
            var config1 = UseNonGenericConfiguratorFor<Dummy>(cfg => cfg.UsingIndexMode(IndexMode.Exclusive));
            var config2 = UseNonGenericConfiguratorFor<Dummy>(cfg => cfg.UsingIndexMode(IndexMode.Inclusive));

            config1.IndexMode.Should().Be(IndexMode.Exclusive);
            config2.IndexMode.Should().Be(IndexMode.Inclusive);
        }

        [Test]
        public void Generic_Should_assign_IndexMode()
        {
            var config1 = UseGenericConfiguratorFor<Dummy>(cfg => cfg.UsingIndexMode(IndexMode.Exclusive));
            var config2 = UseGenericConfiguratorFor<Dummy>(cfg => cfg.UsingIndexMode(IndexMode.Inclusive));

            config1.IndexMode.Should().Be(IndexMode.Exclusive);
            config2.IndexMode.Should().Be(IndexMode.Inclusive);
        }

        [Test]
        public void NonGeneric_Should_store_member_paths_When_defining_specific_members()
        {
            var config = UseNonGenericConfiguratorFor<Dummy>(cfg => cfg.Members("Int1", "String1", "Nested.Int1", "Nested.String1"));

            var memberPaths = config.MemberPaths.ToArray();
            memberPaths.Should().HaveCount(4);
            memberPaths.Should().BeEquivalentTo("Int1", "String1", "Nested.Int1", "Nested.String1");
        }

        [Test]
        public void Generic_Should_store_member_paths_When_defining_specific_members()
        {
            var config = UseGenericConfiguratorFor<Dummy>(cfg => cfg.Members(x => x.Int1, x => x.String1, x => x.Nested.Int1, x => x.Nested.String1));

            var memberPaths = config.MemberPaths.ToArray();
            memberPaths.Should().HaveCount(4);
            memberPaths.Should().BeEquivalentTo("Int1", "String1", "Nested.Int1", "Nested.String1");
        }

        [Test]
        public void NonGeneric_Should_only_store_member_once_When_called_twice()
        {
            var config = UseNonGenericConfiguratorFor<Dummy>(cfg => cfg.Members("String1", "String1"));

            config.MemberPaths.Should().HaveCount(1);
        }

        [Test]
        public void Generic_Should_only_store_member_once_When_called_twice()
        {
            var config = UseGenericConfiguratorFor<Dummy>(cfg => cfg.Members(x => x.String1, x => x.String1));

            config.MemberPaths.Should().HaveCount(1);
        }

        private class Dummy
        {
            public int Int1 { get; set; }

            public string String1 { get; set; }

            public Nested Nested { get; set; }
        }

        private class Nested
        {
            public int Int1 { get; set; }

            public string String1 { get; set; }
        }
    }
}