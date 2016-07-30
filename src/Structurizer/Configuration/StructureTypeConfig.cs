using System;
using System.Collections.Generic;
using EnsureThat;

namespace Structurizer.Configuration
{
    public class StructureTypeConfig : IStructureTypeConfig
    {
        private readonly HashSet<string> _memberPathsBeingIndexed;
        private readonly HashSet<string> _memberPathsNotBeingIndexed;

        public Type Type { get; }
        public bool IndexConfigIsEmpty => MemberPathsBeingIndexed.Count < 1 && MemberPathsNotBeingIndexed.Count < 1;
        public bool IncludeContainedStructureMembers { get; set; }
        public ISet<string> MemberPathsBeingIndexed => _memberPathsBeingIndexed;
        public ISet<string> MemberPathsNotBeingIndexed => _memberPathsNotBeingIndexed;

        public StructureTypeConfig(Type structureType)
        {
            Ensure.That(structureType, "structureType").IsNotNull();

            Type = structureType;

            _memberPathsBeingIndexed = new HashSet<string>();
            _memberPathsNotBeingIndexed = new HashSet<string>();
            IncludeContainedStructureMembers = false;
        }
    }
}