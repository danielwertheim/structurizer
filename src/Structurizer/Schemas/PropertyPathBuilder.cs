namespace Structurizer.Schemas
{
    public static class PropertyPathBuilder
    {
        public static string BuildPath(IStructureProperty property) => property.IsRootMember
            ? property.Name
            : string.Concat(BuildPath(property.Parent), ".", property.Name);

        public static string BuildPath(IStructureProperty parent, string name) => parent == null
            ? name
            : string.Concat(parent.Path, ".", name);
    }
}