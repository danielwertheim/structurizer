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
            var shouldIndexAllMembers = typeConfig.IndexMode == IndexMode.Exclusive && !typeConfig.MemberPaths.Any();

            if (shouldIndexAllMembers)
                return new StructureType(
                    typeConfig.Type,
                    Reflecter.GetIndexableProperties(typeConfig.Type));

            return new StructureType(
                typeConfig.Type,
                typeConfig.IndexMode == IndexMode.Exclusive
                    ? Reflecter.GetIndexablePropertiesExcept(typeConfig.Type, typeConfig.MemberPaths.ToList())
                    : Reflecter.GetSpecificIndexableProperties(typeConfig.Type, typeConfig.MemberPaths.ToList()));
        }
    }
}