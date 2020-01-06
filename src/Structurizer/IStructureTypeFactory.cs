namespace Structurizer
{
    public interface IStructureTypeFactory
    {
        IStructureType CreateFor(IStructureTypeConfig typeConfig);
    }
}