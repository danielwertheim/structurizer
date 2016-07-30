using System;
using Structurizer.Configuration;
using Structurizer.Schemas;

namespace Structurizer
{
    public class StructureTypeFactory : IStructureTypeFactory
    {
        public IStructureTypeReflecter Reflecter { get; }

        public IStructureTypeConfigurations Configurations { get; }

        public StructureTypeFactory(
            IStructureTypeReflecter reflecter = null,
            IStructureTypeConfigurations configurations = null)
        {
            Reflecter = reflecter ?? new StructureTypeReflecter();
            Configurations = configurations ?? new StructureTypeConfigurations();
        }

        public IStructureType CreateFor<T>() where T : class => CreateFor(typeof(T));

        public IStructureType CreateFor(Type structureType)
        {
            var config = Configurations.GetConfiguration(structureType);
            var reflecter = Reflecter;
            var shouldIndexAllMembers = config.IndexConfigIsEmpty;

            if (shouldIndexAllMembers)
                return new StructureType(
                    structureType,
                    reflecter.GetIndexableProperties(structureType));

            var shouldExcludeMembers = config.MemberPathsNotBeingIndexed.Count > 0;
            return new StructureType(
                structureType,
                shouldExcludeMembers
                    ? reflecter.GetIndexablePropertiesExcept(structureType, config.MemberPathsNotBeingIndexed)
                    : reflecter.GetSpecificIndexableProperties(structureType, config.MemberPathsBeingIndexed));
        }
    }
}