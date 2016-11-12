using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EnsureThat;
using Structurizer.Extensions;

namespace Structurizer.Schemas
{
    public class StructureTypeReflecter : IStructureTypeReflecter
    {
        private static readonly string[] EmptyArray = new string[0];

        public const BindingFlags PropertyBindingFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;

        protected IStructurePropertyFactory PropertyFactory { get; }

        public StructureTypeReflecter(IStructurePropertyFactory propertyFactory = null)
        {
            PropertyFactory = propertyFactory ?? new StructurePropertyFactory();
        }

        public IStructureProperty[] GetIndexableProperties(Type structureType)
        {
            return GetIndexableProperties(structureType, null, EmptyArray, null);
        }

        public IStructureProperty[] GetIndexablePropertiesExcept(Type structureType, IList<string> memberPaths)
        {
            Ensure.That(memberPaths, nameof(memberPaths)).HasItems();

            return GetIndexableProperties(structureType, null, memberPaths, null);
        }

        public IStructureProperty[] GetSpecificIndexableProperties(Type structureType, IList<string> memberPaths)
        {
            Ensure.That(memberPaths, nameof(memberPaths)).HasItems();

            var sb = new StringBuilder();
            var paths = new HashSet<string>();
            foreach (var path in memberPaths)
            {
                sb.Clear();
                var parts = path.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    sb.Append(part);
                    paths.Add(sb.ToString());
                    sb.Append(".");
                }
            }

            return GetIndexableProperties(structureType, null, EmptyArray, paths);
        }

        private IStructureProperty[] GetIndexableProperties(
            IReflect type,
            IStructureProperty parent,
            ICollection<string> nonIndexablePaths,
            ICollection<string> indexablePaths)
        {
            var initialPropertyInfos = GetIndexablePropertyInfos(type);
            if (initialPropertyInfos.Length == 0)
                return new IStructureProperty[] { };

            var properties = new List<IStructureProperty>();

            var simplePropertyInfos = GetSimpleIndexablePropertyInfos(initialPropertyInfos, parent, nonIndexablePaths, indexablePaths);
            properties.AddRange(simplePropertyInfos.Select(spi => PropertyFactory.CreateChildPropertyFrom(parent, spi)));

            initialPropertyInfos = initialPropertyInfos.Where(p => !simplePropertyInfos.Contains(p)).ToArray();

            foreach (var complexPropertyInfo in GetComplexIndexablePropertyInfos(initialPropertyInfos, parent, nonIndexablePaths, indexablePaths))
            {
                var complexProperty = PropertyFactory.CreateChildPropertyFrom(parent, complexPropertyInfo);
                var simpleComplexProps = GetIndexableProperties(
                    complexProperty.DataType, complexProperty, nonIndexablePaths, indexablePaths);

                var beforeCount = properties.Count;
                properties.AddRange(simpleComplexProps);

                if (properties.Count == beforeCount && complexProperty.DataType.IsValueType)
                    properties.Add(complexProperty);
            }

            foreach (var enumerablePropertyInfo in GetEnumerableIndexablePropertyInfos(initialPropertyInfos, parent, nonIndexablePaths, indexablePaths))
            {
                var enumerableProperty = PropertyFactory.CreateChildPropertyFrom(parent, enumerablePropertyInfo);
                if (enumerableProperty.ElementDataType.IsSimpleType())
                {
                    properties.Add(enumerableProperty);
                    continue;
                }

                var elementProperties = GetIndexableProperties(
                    enumerableProperty.ElementDataType,
                    enumerableProperty,
                    nonIndexablePaths,
                    indexablePaths);

                properties.AddRange(elementProperties);
            }

            return properties.ToArray();
        }

        private static PropertyInfo[] GetIndexablePropertyInfos(IReflect type) => type.GetProperties(PropertyBindingFlags);

        private static PropertyInfo[] GetSimpleIndexablePropertyInfos(
            PropertyInfo[] properties,
            IStructureProperty parent = null,
            ICollection<string> nonIndexablePaths = null,
            ICollection<string> indexablePaths = null)
        {
            if (properties.Length == 0)
                return new PropertyInfo[0];

            var filteredProperties = properties.Where(p => p.PropertyType.IsSimpleType());

            if (nonIndexablePaths != null && nonIndexablePaths.Any())
                filteredProperties = filteredProperties.Where(p => !nonIndexablePaths.Contains(
                    PropertyPathBuilder.BuildPath(parent, p.Name)));

            if (indexablePaths != null && indexablePaths.Any())
                filteredProperties = filteredProperties.Where(p => indexablePaths.Contains(
                    PropertyPathBuilder.BuildPath(parent, p.Name)));

            return filteredProperties.ToArray();
        }

        private static PropertyInfo[] GetComplexIndexablePropertyInfos(
            PropertyInfo[] properties,
            IStructureProperty parent = null,
            ICollection<string> nonIndexablePaths = null,
            ICollection<string> indexablePaths = null)
        {
            if (properties.Length == 0)
                return new PropertyInfo[0];

            var filteredProperties = properties.Where(p =>
                !p.PropertyType.IsSimpleType() &&
                !p.PropertyType.IsEnumerableType());

            if (nonIndexablePaths != null && nonIndexablePaths.Any())
                filteredProperties = filteredProperties.Where(p => !nonIndexablePaths.Contains(
                    PropertyPathBuilder.BuildPath(parent, p.Name)));

            if (indexablePaths != null && indexablePaths.Any())
                filteredProperties = filteredProperties.Where(p => indexablePaths.Contains(
                    PropertyPathBuilder.BuildPath(parent, p.Name)));

            return filteredProperties.ToArray();
        }

        private static PropertyInfo[] GetEnumerableIndexablePropertyInfos(
            PropertyInfo[] properties,
            IStructureProperty parent = null,
            ICollection<string> nonIndexablePaths = null,
            ICollection<string> indexablePaths = null)
        {
            if (properties.Length == 0)
                return new PropertyInfo[0];

            var filteredProperties = properties.Where(p =>
                !p.PropertyType.IsSimpleType() &&
                p.PropertyType.IsEnumerableType() &&
                !p.PropertyType.IsEnumerableBytesType());

            if (nonIndexablePaths != null && nonIndexablePaths.Any())
                filteredProperties = filteredProperties.Where(p => !nonIndexablePaths.Contains(
                    PropertyPathBuilder.BuildPath(parent, p.Name)));

            if (indexablePaths != null && indexablePaths.Any())
                filteredProperties = filteredProperties.Where(p => indexablePaths.Contains(
                    PropertyPathBuilder.BuildPath(parent, p.Name)));

            return filteredProperties.ToArray();
        }

        //private IStructureProperty[] GetIndexableProperties(
        //    IReflect type,
        //    IStructureProperty parent,
        //    ICollection<string> nonIndexablePaths,
        //    ICollection<string> indexablePaths)
        //{
        //    var initialPropertyInfos = GetIndexablePropertyInfos(type);
        //    if (initialPropertyInfos.Length == 0)
        //        return new IStructureProperty[] { };

        //    var properties = new List<IStructureProperty>();

        //    var simplePropertyInfos = GetSimpleIndexablePropertyInfos(initialPropertyInfos, parent, nonIndexablePaths, indexablePaths);
        //    properties.AddRange(simplePropertyInfos.Select(spi => PropertyFactory.CreateChildPropertyFrom(parent, spi)));

        //    initialPropertyInfos = initialPropertyInfos.Where(p => !simplePropertyInfos.Contains(p)).ToArray();

        //    foreach (var complexPropertyInfo in GetComplexIndexablePropertyInfos(initialPropertyInfos, parent, nonIndexablePaths, indexablePaths))
        //    {
        //        var complexProperty = PropertyFactory.CreateChildPropertyFrom(parent, complexPropertyInfo);
        //        var simpleComplexProps = GetIndexableProperties(
        //            complexProperty.DataType, complexProperty, nonIndexablePaths, indexablePaths);

        //        var beforeCount = properties.Count;
        //        properties.AddRange(simpleComplexProps);

        //        if (properties.Count == beforeCount && complexProperty.DataType.IsValueType)
        //            properties.Add(complexProperty);
        //    }

        //    foreach (var enumerablePropertyInfo in GetEnumerableIndexablePropertyInfos(initialPropertyInfos, parent, nonIndexablePaths, indexablePaths))
        //    {
        //        var enumerableProperty = PropertyFactory.CreateChildPropertyFrom(parent, enumerablePropertyInfo);
        //        if (enumerableProperty.ElementDataType.IsSimpleType())
        //        {
        //            properties.Add(enumerableProperty);
        //            continue;
        //        }

        //        var elementProperties = GetIndexableProperties(
        //            enumerableProperty.ElementDataType,
        //            enumerableProperty,
        //            nonIndexablePaths,
        //            indexablePaths);

        //        properties.AddRange(elementProperties);
        //    }

        //    return properties.ToArray();
        //}

        //private static PropertyInfo[] GetIndexablePropertyInfos(IReflect type) => type.GetProperties(PropertyBindingFlags);

        //private PropertyInfo[] GetSimpleIndexablePropertyInfos(
        //    PropertyInfo[] properties,
        //    IStructureProperty parent = null,
        //    ICollection<string> nonIndexablePaths = null,
        //    ICollection<string> indexablePaths = null)
        //{
        //    if (properties.Length == 0)
        //        return new PropertyInfo[0];

        //    var filteredProperties = properties.Where(p => p.PropertyType.IsSimpleType());

        //    if (nonIndexablePaths != null && nonIndexablePaths.Any())
        //        filteredProperties = filteredProperties.Where(p => !nonIndexablePaths.Contains(
        //            PropertyPathBuilder.BuildPath(parent, p.Name)));

        //    if (indexablePaths != null && indexablePaths.Any())
        //        filteredProperties = filteredProperties.Where(p => indexablePaths.Contains(
        //            PropertyPathBuilder.BuildPath(parent, p.Name)));

        //    return filteredProperties.ToArray();
        //}

        //private PropertyInfo[] GetComplexIndexablePropertyInfos(
        //    PropertyInfo[] properties,
        //    IStructureProperty parent = null,
        //    ICollection<string> nonIndexablePaths = null,
        //    ICollection<string> indexablePaths = null)
        //{
        //    if (properties.Length == 0)
        //        return new PropertyInfo[0];

        //    var filteredProperties = properties.Where(p =>
        //        !p.PropertyType.IsSimpleType() &&
        //        !p.PropertyType.IsEnumerableType());

        //    if (nonIndexablePaths != null && nonIndexablePaths.Any())
        //        filteredProperties = filteredProperties.Where(p => !nonIndexablePaths.Contains(
        //            PropertyPathBuilder.BuildPath(parent, p.Name)));

        //    if (indexablePaths != null && indexablePaths.Any())
        //        filteredProperties = filteredProperties.Where(p => indexablePaths.Contains(
        //            PropertyPathBuilder.BuildPath(parent, p.Name)));

        //    return filteredProperties.ToArray();
        //}

        //private PropertyInfo[] GetEnumerableIndexablePropertyInfos(
        //    PropertyInfo[] properties,
        //    IStructureProperty parent = null,
        //    ICollection<string> nonIndexablePaths = null,
        //    ICollection<string> indexablePaths = null)
        //{
        //    if (properties.Length == 0)
        //        return new PropertyInfo[0];

        //    var filteredProperties = properties.Where(p =>
        //        !p.PropertyType.IsSimpleType() &&
        //        p.PropertyType.IsEnumerableType() &&
        //        !p.PropertyType.IsEnumerableBytesType());

        //    if (nonIndexablePaths != null && nonIndexablePaths.Any())
        //        filteredProperties = filteredProperties.Where(p => !nonIndexablePaths.Contains(
        //            PropertyPathBuilder.BuildPath(parent, p.Name)));

        //    if (indexablePaths != null && indexablePaths.Any())
        //        filteredProperties = filteredProperties.Where(p => indexablePaths.Contains(
        //            PropertyPathBuilder.BuildPath(parent, p.Name)));

        //    return filteredProperties.ToArray();
        //}
    }
}