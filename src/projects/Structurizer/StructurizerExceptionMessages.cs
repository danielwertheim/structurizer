namespace Structurizer
{
    public static class StructurizerExceptionMessages
    {
        public static readonly string AutoSchemaBuilder_MissingIndexableMembers
            = "The Item of type '{0}' has no members that are indexable. There's no point in treating items that has nothing to index.";

        public static readonly string ExpressionEvaluation_DontKnowHowToEvalExpression
            = "Don't know how to evaluate the expression type: '{0}'.";
        public static readonly string ExpressionEvaluation_DontKnowHowToEvalUnaryExpression
            = "Don't know how to evaluate the unary expression of node type: '{0}'.";

        public static readonly string TypeExtensions_ExtractEnumerableGenericType
            = "When extracting generic element type from enumerables, a maximum of two generic arguments are supported, which then are supposed to belong to KeyValuePair<TKey, TValue>.";
    }
}