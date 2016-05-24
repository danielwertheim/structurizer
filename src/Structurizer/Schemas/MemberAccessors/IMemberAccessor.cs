using System;

namespace Structurizer.Schemas.MemberAccessors
{
    public interface IMemberAccessor
    {
        string Path { get; }
        Type DataType { get; }
    }
}