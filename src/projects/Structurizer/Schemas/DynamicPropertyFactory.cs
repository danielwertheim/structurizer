using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Structurizer.Extensions;

namespace Structurizer.Schemas
{
    public static class DynamicPropertyFactory
    {
        private static readonly Type ObjectType = typeof(object);
        private static readonly Type IlGetterType = typeof(Func<object, object>);

        public static DynamicGetter GetterFor(PropertyInfo p)
        {
            if (p.DeclaringType.IsKeyValuePairType())
                return new DynamicGetter(CreateLambdaGetter(p.DeclaringType, p));

            return new DynamicGetter(CreateIlGetter(p));
        }

        private static Func<object, object> CreateLambdaGetter(Type type, PropertyInfo property)
        {
            var objExpr = Expression.Parameter(ObjectType, "theItem");
            var castedObjExpr = Expression.Convert(objExpr, type);

            var p = Expression.Property(castedObjExpr, property);
            var castedProp = Expression.Convert(p, ObjectType);

            var lambda = Expression.Lambda<Func<object, object>>(castedProp, objExpr);

            return lambda.Compile();
        }

        private static Func<object, object> CreateIlGetter(PropertyInfo propertyInfo)
        {
            var propGetMethod = propertyInfo.GetGetMethod(true);
            if (propGetMethod == null)
                return null;

            var getter = CreateDynamicGetMethod(propertyInfo);

            var generator = getter.GetILGenerator();
            generator.DeclareLocal(ObjectType);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            generator.EmitCall(OpCodes.Callvirt, propGetMethod, (Type[])null);

            if (!propertyInfo.PropertyType.IsClass)
                generator.Emit(OpCodes.Box, propertyInfo.PropertyType);

            generator.Emit(OpCodes.Ret);

            return (Func<object, object>)getter.CreateDelegate(IlGetterType);
        }

        private static DynamicMethod CreateDynamicGetMethod(PropertyInfo propertyInfo)
        {
            var args = new[] { ObjectType };
            var name = $"_Get{propertyInfo.Name}_";
            var returnType = ObjectType;

            return !propertyInfo.DeclaringType.IsInterface
                       ? new DynamicMethod(
                             name,
                             returnType,
                             args,
                             propertyInfo.DeclaringType,
                             true)
                       : new DynamicMethod(
                             name,
                             returnType,
                             args,
                             propertyInfo.Module,
                             true);
        }
    }
}