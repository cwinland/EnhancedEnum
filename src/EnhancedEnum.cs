// ***********************************************************************
// Assembly         : EnhancedEnum
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-13-2020
// ***********************************************************************
// <copyright file="EnhancedEnum.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EnhancedEnum
{
    /// <summary>
    /// Inherit to enable Enum behavior object with auto string and value conversion.
    /// Implements the <see cref="EnhancedEnum.IEnhancedEnum{TValue, TDerived}" />
    /// </summary>
    /// <typeparam name="TValue">The type of the t value.</typeparam>
    /// <typeparam name="TDerived">The type of the t derived.</typeparam>
    /// <seealso cref="EnhancedEnum.IEnhancedEnum{TValue, TDerived}" />
    /// <example>
    ///   <code>
    /// public sealed class StatusTest : EnhancedEnum&lt;int, StatusTest&gt;
    /// {
    /// [Description("Indicates Running")] // Description Property.
    /// [DisplayName("In Process")]        // Displays this text when converted to a string.
    /// [Value(5)]                         // Underlying value.
    /// public static readonly StatusTest Running = new StatusTest();
    /// // Gets the default value of 1. This can be overridden with the Value Attribute.
    /// public static readonly StatusTest Stopped = new StatusTest();
    /// // Gets the default value of 2. This can be overridden with the Value Attribute.
    /// public static readonly StatusTest Error = new StatusTest();
    /// // Operators allow conversion to this class from other values.
    /// public static implicit operator StatusTest(string value) =&gt; Convert(value);
    /// public static implicit operator StatusTest(int value) =&gt; Convert(value);
    /// }</code>
    /// </example>
    public abstract class EnhancedEnum<TValue, TDerived>
                        : IEnhancedEnum<TValue, TDerived>
                        where TValue : IComparable<TValue>, IEquatable<TValue>
                        where TDerived : EnhancedEnum<TValue, TDerived>
    {
        #region Fields

        private static bool isSetup;
        private static SortedList<long, TDerived> values;
        private DescriptionAttribute descriptionAttribute;
        private DisplayNameAttribute displayNameAttribute;
        private string name;
        private ValueAttribute valueAttribute;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedEnum{TValue, TDerived}" /> class.
        /// </summary>
        /// <exception cref="ArgumentException">Value already exists. Value parameter must be unique. - autoValue</exception>
        protected EnhancedEnum()
        {
            values ??= new SortedList<long, TDerived>();
            var autoValue = DateTime.Now.Ticks;
            if (values.ContainsKey(autoValue))
            {
                throw new ArgumentException("Value already exists. Value parameter must be unique.", nameof(autoValue));
            }
            //Value = value;
            values.Add(autoValue, (TDerived)this);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public static int Count => values.Count;

        /// <summary>
        /// Enumerable list of all possible values.
        /// </summary>
        /// <value>The values.</value>
        public static IEnumerable<TDerived> Values => values.Values;

        /// <summary>
        /// Description of enumeration as described in <see cref="DescriptionAttribute" />
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                EnsureSetup();

                return descriptionAttribute?.Description ?? Name;
            }
        }

        /// <summary>
        /// Name of enumeration as described in <see cref="DisplayNameAttribute" />
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                EnsureSetup();
                return displayNameAttribute?.Name ?? name;
            }
        }

        /// <summary>
        /// Value of enumeration as described in <see cref="ValueAttribute" />
        /// </summary>
        /// <value>The value.</value>
        public TValue Value
        {
            get
            {
                EnsureSetup();

                return (TValue)valueAttribute?.Value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>TDerived.</returns>
        public static TDerived Convert(string value) => Values.FirstOrDefault(x => x.Name == value);

        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>TDerived.</returns>
        public static TDerived Convert(TValue value) => Values.FirstOrDefault(x => x.Value.Equals(value));

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String" /> to <see cref="EnhancedEnum{TValue, TDerived}" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator EnhancedEnum<TValue, TDerived>(string value) =>
                            Values.FirstOrDefault(x => x.Name == value);

        /// <summary>
        /// Performs an implicit conversion from <see cref="TValue" /> to <see cref="EnhancedEnum{TValue, TDerived}" />.
        /// </summary>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator EnhancedEnum<TValue, TDerived>(TValue value2) =>
            Values.FirstOrDefault(x => x.Value.Equals(value2));

        /// <summary>
        /// Performs an implicit conversion from <see cref="EnhancedEnum{TValue, TDerived}" /> to <see cref="System.String" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(EnhancedEnum<TValue, TDerived> value) => value.Name;

        /// <summary>
        /// Performs an implicit conversion from <see cref="EnhancedEnum{TValue, TDerived}" /> to <see cref="TValue" />.
        /// </summary>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TValue(EnhancedEnum<TValue, TDerived> value2) => value2.Value;

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => !(left == right);

        /// <summary>
        /// Implements the &lt; operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) =>
            left.Value == null ? right.Value is object : left.Value.CompareTo(right.Value) < 0;

        /// <summary>
        /// Implements the &lt;= operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) =>
            left.Value == null || left.Value.CompareTo(right.Value) <= 0;

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) =>
            left?.Equals(right) ?? right is null;

        /// <summary>
        /// Implements the &gt; operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) =>
            left.Value is object && left.Value.CompareTo(right.Value) > 0;

        /// <summary>
        /// Implements the &gt;= operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) =>
            left.Value == null ? right.Value == null : left.Value.CompareTo(right.Value) >= 0;

        /// <summary>
        /// Tries the convert.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool TryConvert(string value, out TDerived result)
        {
            result = Values.FirstOrDefault(x => x.Name == value);

            return result != null;
        }

        /// <summary>
        /// Tries the convert.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool TryConvert(TValue value, out TDerived result)
        {
            result = Values.FirstOrDefault(x => x.Value.Equals(value));
            return result != null;
        }

        /// <summary>
        ///   Compares the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>System.Int32.</returns>
        int IComparer<TDerived>.Compare(TDerived x, TDerived y) =>
                            x == null
                ? -1
                : y == null
                    ? 1
                    : string.Compare(x.Name, y.Name, StringComparison.Ordinal);

        /// <summary>
        ///   Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        int IComparable<TDerived>.CompareTo(TDerived other) => Value.CompareTo(other.Value);

        /// <summary>
        ///   Compares to.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.Int32.</returns>
        int IComparable.CompareTo(object obj)
        {
            return obj switch
            {
                null => -1,
                string localValue => string.Compare(Name, localValue, StringComparison.Ordinal),
                TDerived @enum => string.Compare(Name, @enum.Name, StringComparison.Ordinal),
                _ => -1,
            };
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj switch
            {
                string checkName => Name.Equals(checkName),
                TDerived @enum => Name.Equals(@enum.Name),
                _ => false,
            };
        }

        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        bool IEquatable<TDerived>.Equals(TDerived other) => other is { } && Name.Equals(other.Name);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode() => Name.GetHashCode();

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() => Name;

        /// <summary>
        ///   Ensures the setup.
        /// </summary>
        /// <exception cref="ArgumentException">Provided value must be of type 'TValue' - TValue</exception>
        private static void EnsureSetup()
        {
            if (isSetup)
            {
                return;
            }

            var fields = typeof(TDerived)
                         .GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public)
                         .Where(t => t.FieldType == typeof(TDerived));

            var valCount = 0;
            foreach (var field in fields)
            {
                var instance = (TDerived)field.GetValue(null);

                if (instance == null)
                {
                    continue;
                }

                valCount++;
                instance.name = field.Name;
                instance.descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();
                instance.displayNameAttribute = field.GetCustomAttribute<DisplayNameAttribute>();
                instance.valueAttribute = field.GetCustomAttribute<ValueAttribute>() ?? new ValueAttribute(valCount);

                if (!(instance.valueAttribute?.Value is TValue))
                {
                    throw new ArgumentException("Provided value must be of type 'TValue'", nameof(TValue));
                }
            }

            isSetup = true;
        }

        #endregion Methods
    }
}
