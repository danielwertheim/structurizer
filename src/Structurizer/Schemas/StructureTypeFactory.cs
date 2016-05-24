using System;
using Structurizer.Schemas.Configuration;

namespace Structurizer.Schemas
{
    public class StructureTypeFactory : IStructureTypeFactory
    {
        public Func<IStructureTypeConfig, IStructureTypeReflecter> ReflecterFn { get; set; }

        public IStructureTypeConfigurations Configurations { get; set; }

        public StructureTypeFactory(Func<IStructureTypeConfig, IStructureTypeReflecter> reflecterFn = null, IStructureTypeConfigurations configurations = null)
        {
            ReflecterFn = reflecterFn ?? (cfg => new StructureTypeReflecter());
            Configurations = configurations ?? new StructureTypeConfigurations();
        }

        public IStructureType CreateFor<T>() where T : class
        {
            return CreateFor(typeof(T));
        }

        public IStructureType CreateFor(Type structureType)
        {
            var config = Configurations.GetConfiguration(structureType);
            var reflecter = ReflecterFn(config);
            var shouldIndexAllMembers = config.IndexConfigIsEmpty;

            if (shouldIndexAllMembers)
                return new StructureType(
                    structureType,
                    reflecter.GetIndexableProperties(structureType));

            var shouldIndexAllMembersExcept = config.MemberPathsNotBeingIndexed.Count > 0;
            return new StructureType(
                structureType,
                shouldIndexAllMembersExcept
                    ? reflecter.GetIndexablePropertiesExcept(structureType, config.MemberPathsNotBeingIndexed)
                    : reflecter.GetSpecificIndexableProperties(structureType, config.MemberPathsBeingIndexed));
        }
    }
}