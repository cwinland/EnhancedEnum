using System;
using System.Collections.Generic;
using System.Text;

namespace EnhancedEnum
{
    public interface IEnhancedEnum<TValue, TDerived> : IEquatable<TDerived>,
                                                       IComparable<TDerived>,
                                                       IComparable, IComparer<TDerived>

    {
        string Description { get; }
        string Name { get; }
        TValue Value { get; }
    }
}
