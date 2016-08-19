using System;
using System.Linq.Expressions;

namespace Structurizer
{
    public interface IStructureTypeConfigurator
    {
        IStructureTypeConfigurator UsingIndexMode(IndexMode mode);
        IStructureTypeConfigurator Members(params string[] memberPaths);
        IStructureTypeConfig GenerateConfig();
    }

    public interface IStructureTypeConfigurator<T> where T : class
    {
        IStructureTypeConfigurator<T> UsingIndexMode(IndexMode mode);
        IStructureTypeConfigurator<T> Members(params string[] memberPaths);
        IStructureTypeConfigurator<T> Members(params Expression<Func<T, object>>[] members);
        IStructureTypeConfig GenerateConfig();
    }
}