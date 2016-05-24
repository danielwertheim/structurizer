using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Structurizer.Extensions
{
    namespace NCore.Expressions
    {
        public static class ExpressionExtensions
        {
            public static object Evaluate(this Expression e)
            {
                if (e is MethodCallExpression)
                    return (e as MethodCallExpression).Evaluate();

                if (e is MemberExpression)
                    return (e as MemberExpression).Evaluate();

                if (e is ConstantExpression)
                    return (e as ConstantExpression).Evaluate();

                if (e is NewArrayExpression)
                    return (e as NewArrayExpression).Evaluate();

                if (e is UnaryExpression)
                    return (e as UnaryExpression).Evaluate();

                throw new StructurizerException(
                    string.Format(StructurizerExceptionMessages.ExpressionEvaluation_DontKnowHowToEvalExpression, e.GetType().Name));
            }

            public static object[] Evaluate(this NewArrayExpression e)
            {
                return e.Expressions.Select(ie => ie.Evaluate()).ToArray();
            }

            public static object Evaluate(this UnaryExpression e)
            {
                if (e.Operand is ConstantExpression)
                    return (e.Operand as ConstantExpression).Evaluate();

                if (e.Operand is MethodCallExpression)
                    return (e.Operand as MethodCallExpression).Evaluate();

                throw new StructurizerException(
                    string.Format(StructurizerExceptionMessages.ExpressionEvaluation_DontKnowHowToEvalUnaryExpression, e.NodeType));
            }

            public static object Evaluate(this MethodCallExpression methodExpression)
            {
                if (methodExpression.Object == null)
                {
                    var args = methodExpression.Arguments.OfType<ConstantExpression>().Select(c => c.Value).ToArray();
                    if (args.Length == methodExpression.Arguments.Count)
                        return methodExpression.Method.Invoke(null, args);
                }

                return Expression.Lambda(methodExpression).Compile().DynamicInvoke();
            }

            public static object Evaluate(this MemberExpression memberExpression)
            {
                if (memberExpression.Member.MemberType == MemberTypes.Field && memberExpression.Expression is ConstantExpression)
                {
                    var ce = (ConstantExpression)memberExpression.Expression;
                    var obj = ce.Value;
                    return obj == null
                        ? null
                        : ((FieldInfo)memberExpression.Member).GetValue(obj);
                }
                return Expression.Lambda(memberExpression).Compile().DynamicInvoke();
            }

            public static object Evaluate(this ConstantExpression constantExpression)
            {
                return constantExpression.Value;
            }

            public static MemberExpression GetRightMostMember(this Expression e)
            {
                if (e is LambdaExpression)
                    return GetRightMostMember(((LambdaExpression)e).Body);

                if (e is MemberExpression)
                    return (MemberExpression)e;

                if (e is MethodCallExpression)
                {
                    var callExpression = (MethodCallExpression)e;

                    if (callExpression.Object is MethodCallExpression || callExpression.Object is MemberExpression)
                        return GetRightMostMember(callExpression.Object);

                    var member = callExpression.Arguments.Count > 0 ? callExpression.Arguments[0] : callExpression.Object;
                    return GetRightMostMember(member);
                }

                if (e is UnaryExpression)
                {
                    var unaryExpression = (UnaryExpression)e;
                    return GetRightMostMember(unaryExpression.Operand);
                }

                return null;
            }

            public static string ToPath(this MemberExpression e)
            {
                var parent = e.Expression as MemberExpression;
                var path = "";

                if (parent != null)
                    path = parent.ToPath() + ".";

                return path + e.Member.Name;
            }
        }
    }
}