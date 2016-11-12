using System.Collections.Generic;
using System.Collections.ObjectModel;
using EnsureThat;

namespace Structurizer
{
    public class Structure : IStructure
    {
        public string Name { get; }
        public IReadOnlyList<IStructureIndex> Indexes { get; }

        public Structure(string name, IStructureIndex[] indexes)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();
            Ensure.That(indexes, nameof(indexes)).IsNotNull();

            Name = name;
            Indexes = new ReadOnlyCollection<IStructureIndex>(indexes);
        }
    }
}