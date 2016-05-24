using System;
using System.Collections.Generic;
using EnsureThat;

namespace Structurizer
{
    [Serializable]
    public class Structure : IStructure
    {
        public string Name { get; }
        public IList<IStructureIndex> Indexes { get; }

        private Structure()
        {
            Indexes = new List<IStructureIndex>();
        }

        public Structure(string name, IStructureIndex[] indexes)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();
            Ensure.That(indexes, nameof(indexes)).HasItems();

            Name = name;
            Indexes = new List<IStructureIndex>(indexes);
        }
    }
}