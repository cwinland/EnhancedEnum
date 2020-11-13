using System;

namespace EnhancedEnum
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ValueAttribute : Attribute
    {
        public object Value { get; }

        public ValueAttribute(object value) => Value = value;
    }
}
