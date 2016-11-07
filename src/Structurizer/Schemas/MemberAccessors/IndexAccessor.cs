using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structurizer.Schemas.MemberAccessors
{
    public class IndexAccessor : MemberAccessorBase, IIndexAccessor
    {
        private delegate void OnLastPropertyFound(IStructureProperty lastProperty, object currentNode);

        private readonly IStructureProperty[] _callstack;

        public DataTypeCode DataTypeCode { get; }

        public bool IsEnumerable => Property.IsEnumerable;

        public bool IsElement => Property.IsElement;

        public IndexAccessor(IStructureProperty property, DataTypeCode dataTypeCode)
            : base(property)
        {
            var callstack = ExtractCallstack(Property);
            callstack.Reverse();
            _callstack = callstack.ToArray();

            DataTypeCode = dataTypeCode;
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

        public IList<INodeValue> GetValues<T>(T item) where T : class
        {
            if (!Property.IsRootMember)
                return EvaluateCallstack(item, startAtCallstackIndex: 0, parentPath: Property.Path);

            var rootMemberValue = Property.GetValue(item);
            if (rootMemberValue == null)
                return null;

            return IsEnumerable
                ? CollectionOfValuesToList((IEnumerable)rootMemberValue)
                : new List<INodeValue> { new NodeValue(Property.Path, rootMemberValue) };
        }

        private IList<INodeValue> EvaluateCallstack<T>(T startNode, int startAtCallstackIndex, string parentPath)
        {
            object currentNode = startNode;
            var maxCallstackIndex = _callstack.Length - 1;
            for (var callstackIndex = startAtCallstackIndex; callstackIndex < _callstack.Length; callstackIndex++)
            {
                if (currentNode == null)
                    return new List<INodeValue> { null };

                var enumerableNode = currentNode as IEnumerable;
                var currentProperty = _callstack[callstackIndex];
                var isLastProperty = callstackIndex == maxCallstackIndex;
                if (isLastProperty)
                    return enumerableNode != null
                        ? ExtractValuesForEnumerableNode(enumerableNode, currentProperty, parentPath)
                        : ExtractValuesForSimpleNode(currentNode, currentProperty);

                if (enumerableNode == null)
                    currentNode = currentProperty.GetValue(currentNode);
                else
                {
                    var values = new List<INodeValue>();
                    var i = -1;
                    foreach (var node in enumerableNode)
                    {
                        i += 1;
                        values.AddRange(EvaluateCallstack(currentProperty.GetValue(node), startAtCallstackIndex: callstackIndex + 1, parentPath: currentProperty.Parent.Path + $"[{i}]"));
                    }
                    return values;
                }
            }

            return null;
        }

        private static IList<INodeValue> ExtractValuesForEnumerableNode<T>(T nodes, IStructureProperty property, string parentPath) where T : IEnumerable
        {
            var collection = nodes as ICollection;
            var values = collection != null
                ? new List<INodeValue>(collection.Count)
                : new List<INodeValue>();

            var i = -1;
            foreach (var node in nodes)
            {
                i += 1;
                var path = $"{parentPath}.{property.Parent.Name}[{i}].{property.Name}";

                if (node == null)
                {
                    values.Add(new NodeValue(path, null));
                    continue;
                }

                var nodeValue = property.GetValue(node);
                if (nodeValue == null)
                {
                    values.Add(new NodeValue(path, null));
                    continue;
                }

                var enumerableNode = nodeValue as IEnumerable;

                if (enumerableNode != null && !(nodeValue is string))
                    values.AddRange(CollectionOfValuesToList(enumerableNode));
                else
                    values.Add(new NodeValue(path, nodeValue));
            }

            return values;
        }

        private static IList<INodeValue> ExtractValuesForSimpleNode(object node, IStructureProperty property)
        {
            var currentValue = property.GetValue(node);

            if (currentValue == null)
                return null;

            if (!property.IsEnumerable)
                return new List<INodeValue> { new NodeValue(property.Path, currentValue) };

            return CollectionOfValuesToList((IEnumerable)currentValue);
        }

        private static IList<INodeValue> CollectionOfValuesToList<T>(T elements) where T : IEnumerable
        {
            var collection = elements as ICollection;
            var values = collection != null
                ? new List<INodeValue>(collection.Count)
                : new List<INodeValue>();

            var i = 0;
            foreach (var element in elements)
            {
                values.Add(new NodeValue("---", element));
                i += 1;
            }

            return values;
        }

        public void SetValue<T>(T item, object value) where T : class
        {
            if (Property.IsRootMember)
            {
                Property.SetValue(item, value);
                return;
            }

            EnumerateToLastProperty(item, startIndex: 0, onLastPropertyFound: (lastProperty, currentNode) => lastProperty.SetValue(currentNode, value));
        }

        private void EnumerateToLastProperty<T>(T startNode, int startIndex, OnLastPropertyFound onLastPropertyFound) where T : class
        {
            object currentNode = startNode;
            var maxCallstackIndex = _callstack.Length - 1;
            for (var c = startIndex; c < _callstack.Length; c++)
            {
                var currentProperty = _callstack[c];
                var isLastPropertyInfo = c == maxCallstackIndex;
                if (isLastPropertyInfo)
                {
                    onLastPropertyFound(currentProperty, currentNode);
                    break;
                }

                currentNode = currentProperty.GetValue(currentNode);
            }
        }
    }
}