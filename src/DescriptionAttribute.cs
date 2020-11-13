using System;

namespace EnhancedEnum
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DescriptionAttribute : Attribute
    {
        public string Description { get; }

        public DescriptionAttribute(string description) => Description = description;
    }
}
