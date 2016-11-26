using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Structurizer.Schemas
{
    public class IndexAccessor : IIndexAccessor
    {
        private readonly StructurePropertyCallstack _callstack;

        protected IStructureProperty Property { get; }
        public string Path => Property.Path;
        public Type DataType => Property.DataType;
        public DataTypeCode DataTypeCode { get; }
        public bool IsEnumerable => Property.IsEnumerable;
        public bool IsElement => Property.IsElement;

        public IndexAccessor(IStructureProperty property, DataTypeCode dataTypeCode)
        {
            _callstack = StructurePropertyCallstack.Generate(property);
            Property = property;
            DataTypeCode = dataTypeCode;
        }

        public IList<IStructureIndexValue> GetValues<T>(T item) where T : class
        {
            if (!Property.IsRootMember)
                return EvaluateCallstack(item, startAtCallstackIndex: 0, startPath: null);

            var rootMemberValue = Property.GetValue(item);
            if (rootMemberValue == null)
                return null;

            return IsEnumerable
                ? CollectionOfValuesToList((IEnumerable)rootMemberValue, Property, startPath: null)
                : new List<IStructureIndexValue> { new StructureIndexValue(Property.Path, rootMemberValue) };
        }

        private IList<IStructureIndexValue> EvaluateCallstack<T>(T startNode, int startAtCallstackIndex, string startPath)
        {
            if (startPath == null)
                startPath = _callstack[0].Name;

            object currentNode = startNode;
            var maxCallstackIndex = _callstack.Length - 1;
            for (var callstackIndex = startAtCallstackIndex; callstackIndex < _callstack.Length; callstackIndex++)
            {
                if (currentNode == null)
                    return new List<IStructureIndexValue> { null };

                var currentProperty = _callstack[callstackIndex];

                var enumerableNode = currentNode as IEnumerable;
                var isLastProperty = callstackIndex == maxCallstackIndex;
                if (isLastProperty)
                    return enumerableNode != null
                        ? ExtractValuesForEnumerableNode(enumerableNode, currentProperty, startPath)
                        : ExtractValuesForSimpleNode(currentNode, currentProperty, startPath);

                if (enumerableNode == null)
                    currentNode = currentProperty.GetValue(currentNode);
                else
                {
                    var values = new List<IStructureIndexValue>();
                    var i = -1;
                    foreach (var node in enumerableNode)
                    {
                        i += 1;
                        var tmpValues = EvaluateCallstack(
                            currentProperty.GetValue(node),
                            startAtCallstackIndex: callstackIndex + 1,
                            startPath: $"{currentProperty.Parent.Path}[{i}].{currentProperty.Name}");

                        //if(tmpValues.Any())
                        values.AddRange(tmpValues);
                    }
                    return values;
                }
            }

            return null;
        }

        private static IList<IStructureIndexValue> ExtractValuesForEnumerableNode<T>(T nodes, IStructureProperty property, string startPath) where T : IEnumerable
        {
            var collection = nodes as ICollection;
            var values = collection != null
                ? new List<IStructureIndexValue>(collection.Count)
                : new List<IStructureIndexValue>();

            var i = -1;
            foreach (var node in nodes)
            {
                i += 1;
                var path = $"{startPath}[{i}]";

                if (node == null)
                {
                    values.Add(new StructureIndexValue($"{path}.{property.Name}", null));
                    continue;
                }

                var nodeValue = property.GetValue(node);
                if (nodeValue == null)
                {
                    values.Add(new StructureIndexValue($"{path}.{property.Name}", null));
                    continue;
                }

                var enumerableNode = nodeValue as IEnumerable;

                if (enumerableNode != null && !(nodeValue is string))
                    values.AddRange(CollectionOfValuesToList(enumerableNode, property, path));
                else
                    values.Add(new StructureIndexValue($"{path}.{property.Name}", nodeValue));
            }

            return values;
        }

        private static IList<IStructureIndexValue> ExtractValuesForSimpleNode(object node, IStructureProperty property, string startPath)
        {
            var currentValue = property.GetValue(node);

            if (currentValue == null)
                return null;

            if (!property.IsEnumerable)
                return new List<IStructureIndexValue> { new StructureIndexValue($"{startPath}.{property.Name}", currentValue) };

            return CollectionOfValuesToList((IEnumerable)currentValue, property, startPath);
        }

        private static IList<IStructureIndexValue> CollectionOfValuesToList<T>(T elements, IStructureProperty property, string startPath) where T : IEnumerable
        {
            var collection = elements as ICollection;
            var values = collection != null
                ? new List<IStructureIndexValue>(collection.Count)
                : new List<IStructureIndexValue>();

            var i = 0;
            var path = new StringBuilder();
            foreach (var element in elements)
            {
                if (startPath != null)
                {
                    path.Append(startPath);
                    path.Append(".");
                }

                path.Append(property.Name);
                path.Append($"[{i}]");

                values.Add(new StructureIndexValue(path.ToString(), element));
                path.Clear();
                i += 1;
            }

            return values;
        }
    }
}