using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EnsureThat;

namespace Structurizer.Configuration
{
    public class StructureTypeConfig : IStructureTypeConfig
    {
        public Type Type { get; }
        public IndexMode IndexMode { get; }
        public IReadOnlyList<string> MemberPaths { get; }

        public StructureTypeConfig(Type structureType, IndexMode indexMode, ISet<string> memberPaths)
        {
            Ensure.That(structureType, nameof(structureType)).IsNotNull();
            Ensure.That(memberPaths, nameof(memberPaths)).IsNotNull();

            Type = structureType;
            IndexMode = indexMode;
            MemberPaths = new ReadOnlyCollection<string>(new List<string>(memberPaths));
        }
    }
}