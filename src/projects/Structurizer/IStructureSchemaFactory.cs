namespace Structurizer
{
    public interface IStructureSchemaFactory
    {
        IStructureSchema CreateSchema(IStructureType structureType);
    }
}