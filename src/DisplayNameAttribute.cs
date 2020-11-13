using System;

namespace EnhancedEnum
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DisplayNameAttribute : Attribute
    {
        public string Name { get; }

        public DisplayNameAttribute(string name) => Name = name;
    }
}
