using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EnsureThat;
using Structurizer.Extensions;

namespace Structurizer.Configuration
{
    public class StructureTypeConfigurator : IStructureTypeConfigurator
    {
        private readonly Type _structureType;
        private readonly ISet<string> _memberPaths = new HashSet<string>();
        private IndexMode _indexMode;

        public StructureTypeConfigurator(Type structureType)
        {
            Ensure.That(structureType, nameof(structureType)).IsNotNull();

            _structureType = structureType;
        }

        public IStructureTypeConfigurator UsingIndexMode(IndexMode mode)
        {
            _indexMode = mode;

            return this;
        }

        public virtual IStructureTypeConfigurator Members(params string[] memberPaths)
        {
            foreach (var memberPath in memberPaths)
                _memberPaths.Add(memberPath);

            return this;
        }

        public IStructureTypeConfig GenerateConfig()
            => new StructureTypeConfig(_structureType, _indexMode, _memberPaths);
    }

    public class StructureTypeConfigurator<T> : IStructureTypeConfigurator<T> where T : class
    {
        private readonly IStructureTypeConfigurator _internalConfigurator;

        public StructureTypeConfigurator(Type structureType)
        {
            _internalConfigurator = new StructureTypeConfigurator(structureType);
        }

        public IStructureTypeConfigurator<T> UsingIndexMode(IndexMode mode)
        {
            _internalConfigurator.UsingIndexMode(mode);

            return this;
        }

        public virtual IStructureTypeConfigurator<T> Members(params string[] memberPaths)
        {
            _internalConfigurator.Members(memberPaths);

            return this;
        }

        public virtual IStructureTypeConfigurator<T> Members(params Expression<Func<T, object>>[] members)
        {
            Members(members
                .Select(e => e.GetRightMostMember().ToPath())
                .ToArray());

            return this;
        }

        public IStructureTypeConfig GenerateConfig() => _internalConfigurator.GenerateConfig();
    }
}