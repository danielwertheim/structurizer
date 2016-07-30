using System.Linq;
using Structurizer.Schemas;

namespace Structurizer
{
    public class StructureTypeFactory : IStructureTypeFactory
    {
        protected IStructureTypeReflecter Reflecter { get; }

        public StructureTypeFactory(
            IStructureTypeReflecter reflecter = null)
        {
            Reflecter = reflecter ?? new StructureTypeReflecter();
        }

        public IStructureType CreateFor(IStructureTypeConfig typeConfig)
        {
            //TODO: Inverse
            var shouldIndexAllMembers = typeConfig.IndexConfigIsEmpty;

            if (shouldIndexAllMembers)
                return new StructureType(
                    typeConfig.Type,
                    Reflecter.GetIndexableProperties(typeConfig.Type));

            var shouldExcludeMembers = typeConfig.MemberPathsNotBeingIndexed.Any();

            return new StructureType(
                typeConfig.Type,
                shouldExcludeMembers
                    ? Reflecter.GetIndexablePropertiesExcept(typeConfig.Type, typeConfig.MemberPathsNotBeingIndexed)
                    : Reflecter.GetSpecificIndexableProperties(typeConfig.Type, typeConfig.MemberPathsBeingIndexed));
        }
    }
}