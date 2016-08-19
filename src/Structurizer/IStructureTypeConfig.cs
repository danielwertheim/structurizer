using System;
using System.Collections.Generic;

namespace Structurizer
{
    public interface IStructureTypeConfig
    {
        Type Type { get; }
        IndexMode IndexMode { get; }
        IReadOnlyList<string> MemberPaths { get; }
    }
}