using System;
using System.Collections.Generic;

namespace Structurizer
{
    public interface IStructureTypeConfig
    {
        Type Type { get; }
        bool IndexConfigIsEmpty { get; }
        ISet<string> MemberPathsBeingIndexed { get; }
        ISet<string> MemberPathsNotBeingIndexed { get; }
    }
}