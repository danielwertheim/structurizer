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
        private static readonly Type VoidType = typeof(void);
        private static readonly Type IlGetterType = typeof(Func<object, object>);
        private static readonly Type IlSetterType = typeof(Action<object, object>);

        public static DynamicGetter GetterFor(PropertyInfo p)
        {
            if (p.DeclaringType.IsKeyValuePairType())
                return new DynamicGetter(CreateLambdaGetter(p.DeclaringType, p));

            return new DynamicGetter(CreateIlGetter(p));
        }

        public static DynamicSetter SetterFor(PropertyInfo p)
        {
            var ilSetter = CreateIlSetter(p);
            return ilSetter == null
                ? null
                : new DynamicSetter(ilSetter);
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

        private static Action<object, object> CreateIlSetter(PropertyInfo propertyInfo)
        {
            var propSetMethod = propertyInfo.GetSetMethod(true);
            if (propSetMethod == null)
                return null;

            var setter = CreateDynamicSetMethod(propertyInfo);

            var generator = setter.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            generator.Emit(OpCodes.Ldarg_1);

            generator.Emit(propertyInfo.PropertyType.IsClass
                ? OpCodes.Castclass
                : OpCodes.Unbox_Any,
                propertyInfo.PropertyType);

            generator.EmitCall(OpCodes.Callvirt, propSetMethod, (Type[])null);
            generator.Emit(OpCodes.Ret);

            return (Action<object, object>)setter.CreateDelegate(IlSetterType);
        }

        private static DynamicMethod CreateDynamicSetMethod(PropertyInfo propertyInfo)
        {
            var args = new[] { ObjectType, ObjectType };
            var name = $"_Set{propertyInfo.Name}_";
            var returnType = VoidType;

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