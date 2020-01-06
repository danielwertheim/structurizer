using System.Collections.Generic;
using EnsureThat;

namespace Structurizer
{
    public class Structure : IStructure
    {
        public string Name { get; }
        public IList<IStructureIndex> Indexes { get; }

        public Structure(string name, IList<IStructureIndex> indexes)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();

            Name = name;
            Indexes = indexes ?? new List<IStructureIndex>(0);
        }
    }
}