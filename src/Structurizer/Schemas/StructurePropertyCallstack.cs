using System.Collections.Generic;

namespace Structurizer.Schemas
{
    public class StructurePropertyCallstack
    {
        private readonly IStructureProperty[] _stack;

        public IStructureProperty this[int index] => _stack[index];
        public int Length => _stack.Length;

        private StructurePropertyCallstack(IStructureProperty[] stack)
        {
            _stack = stack;
        }

        public static StructurePropertyCallstack Generate(IStructureProperty property)
        {
            var callstack = ExtractCallstack(property);

            callstack.Reverse();

            return new StructurePropertyCallstack(callstack.ToArray());
        }

        private static List<IStructureProperty> ExtractCallstack(IStructureProperty property)
        {
            if (property.IsRootMember)
                return new List<IStructureProperty> { property };

            var props = new List<IStructureProperty> { property };
            props.AddRange(
                ExtractCallstack(property.Parent));

            return props;
        }
    }
}