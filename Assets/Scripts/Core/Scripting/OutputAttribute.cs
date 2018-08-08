using System;

namespace pdxpartyparrot.Core.Scripting
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class OutputAttribute : Attribute
    {
    }
}
