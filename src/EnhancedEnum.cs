// ***********************************************************************
// Assembly         : EnhancedEnum
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-16-2020
// ***********************************************************************
// <copyright file="EnhancedEnum.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using EnhancedEnum.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EnhancedEnum
{
    /// <summary>
    /// Inherit to enable Enum behavior object with auto string and value conversion.
    /// Implements the <see cref="IEnhancedEnum{TValue,TDerived}" />
    /// </summary>
    /// <typeparam name="TValue">The type of the t value.</typeparam>
    /// <typeparam name="TDerived">The type of the t derived.</typeparam>
    /// <seealso cref="IEnhancedEnum{TValue,TDerived}" />
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

        private static TDerived defaultDerived;
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
        /// Gets or sets a value indicating whether to throw an Exception (instead of null value) when a conversion error occurs.
        /// </summary>
        /// <value><c>true</c> if [throw on error]; otherwise, <c>false</c>.</value>
        public static bool ThrowOnError { get; set; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public static int Count => values.Count;

        /// <summary>
        /// Gets a value indicating whether this instance is flag.
        /// </summary>
        /// <value><c>true</c> if this instance is flag; otherwise, <c>false</c>.</value>
        public static bool IsFlag =>
            typeof(TDerived)
                .GetCustomAttribute<FlagAttribute>() !=
            null;

        /// <summary>
        /// Enumerable list of all possible values.
        /// </summary>
        /// <value>The values.</value>
        public static List<TDerived> Values => values.Values.ToList();

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

                return IsFlag ? string.Join(",",
                                            values.Where(x => (int)(x.Value.Value as object) > 0 && HasFlag((int) (x.Value.Value as object)))
                                                  .Select(x => x.Value.displayNameAttribute?.Name ?? x.Value.name)
                                                  .ToList())
                    : displayNameAttribute?.Name ?? name;
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
                var convertedType = TypeConverter(valueAttribute?.Value);
                return convertedType.Equals(default) ? (TValue)valueAttribute?.Value : convertedType;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>TDerived.</returns>
        public static TDerived Convert(string value) =>
            ThrowOnError
                ? Values.First(x => (x.displayNameAttribute?.Name ?? x.name) == value) ?? GetFlagDefault(value)
                : Values.FirstOrDefault(x => (x.displayNameAttribute?.Name ?? x.name) == value) ?? GetFlagDefault(value);

        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>TDerived.</returns>
        public static TDerived Convert(TValue value) =>
            ThrowOnError
                ? Values.First(x => x.Value.Equals(value)) ?? GetFlagDefault(value)
                : Values.FirstOrDefault(x => x.Value.Equals(value)) ?? GetFlagDefault(value);

        /// <summary>
        /// Determines whether the specified flags has flag.
        /// </summary>
        /// <param name="flags">The <see cref="EnhancedEnum{TValue,TDerived}" />.</param>
        /// <param name="flag">The flag.</param>
        /// <returns><c>true</c> if the specified flags has flag; otherwise, <c>false</c>.</returns>
        public static bool HasFlag(int flags, int flag) => IsFlag && (flags & flag) != 0;

        /// <summary>
        /// Determines whether the specified flag has flag.
        /// </summary>
        /// <param name="flag">The flag.</param>
        /// <returns><c>true</c> if the specified flag has flag; otherwise, <c>false</c>.</returns>
        public bool HasFlag(int flag)
        {
            if (!IsFlag || !int.TryParse(Value.ToString(), out var flags))
            {
                return false;
            }

            return IsFlag && ((flags & flag) != 0 || flag == 0);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="string" /> to <see cref="EnhancedEnum{TValue, TDerived}" />.
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
        /// Performs an implicit conversion from <see cref="EnhancedEnum{TValue, TDerived}" /> to <see cref="string" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(EnhancedEnum<TValue, TDerived> value) => value.Name;

        /// <summary>
        /// Performs an implicit conversion from <see cref="EnhancedEnum{TValue, TDerived}" /> to <see cref="TValue" />.
        /// </summary>
        /// <param name="value">The value2.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TValue(EnhancedEnum<TValue, TDerived> value) => value.Value;

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
        /// Tries the convert the string to <see cref="TDerived" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if <see cref="TDerived" />, <c>false</c> otherwise.</returns>
        public static bool TryConvert(string value, out TDerived result)
        {
            result = Values.FirstOrDefault(x => x.Name == value);

            return result != null;
        }

        /// <summary>
        /// Tries the convert <see cref="TValue" /> to <see cref="TDerived" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if <see cref="TDerived" />, <c>false</c> otherwise.</returns>
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
        /// Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
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
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        bool IEquatable<TDerived>.Equals(TDerived other) => other is { } && Name.Equals(other.Name);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString() => Name;

        /// <summary>
        /// Types the converter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>TValue.</returns>
        protected virtual TValue TypeConverter(object value) => default;

        private static List<FieldInfo> GetFields() =>
            typeof(TDerived)
                .GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public)
                .Where(t => t.FieldType == typeof(TDerived) && (TDerived) t.GetValue(null) != null)
                .ToList();

        private static IEnumerable<TDerived> GetInstances() =>
            GetFields()
                .Select(field => (TDerived) field.GetValue(null));

        /// <summary>
        ///   Ensures the enum is setup.
        /// </summary>
        /// <exception cref="ArgumentException">Provided value must be of type 'TValue' - TValue</exception>
        private static void EnsureSetup()
        {
            if (isSetup)
            {
                return;
            }

            var valCount = 0; // Count fields

            GetFields()
                .ForEach(
                    field =>
                    {
                        var instance = (TDerived) field.GetValue(null);

                        if (IsFlag && defaultDerived == null)
                        {
                            defaultDerived = instance;
                        }

                        valCount++;
                        instance.name = field.Name;
                        instance.descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();
                        instance.displayNameAttribute = field.GetCustomAttribute<DisplayNameAttribute>();
                        instance.valueAttribute =
                            field.GetCustomAttribute<ValueAttribute>() ?? new ValueAttribute(valCount);

                        var convertedType = instance.TypeConverter(instance.valueAttribute?.Value);
                        var convertedValue = convertedType.Equals(default)
                            ? instance.valueAttribute?.Value
                            : convertedType;

                        if (!(convertedValue is TValue))
                        {
                            throw new ArgumentException("Provided value must be of type 'TValue'", nameof(TValue));
                        }
                    }
                );


            isSetup = true;
        }

        private static TDerived GetFlagDefault(string value)
        {
            if (!IsFlag || string.IsNullOrEmpty(value) && ThrowOnError)
            {
                return null;
            }

            if (string.IsNullOrEmpty(value))
            {
                return GetInstances()
                       .First()
                       .GetValue(0);
            }

            EnsureSetup();
            var list = new List<TDerived>();

            value.Split(',')
                 .ToList()
                 .ForEach(s => list.Add(Convert(s.Trim())));

            var result = 0;

            list.ForEach(x=>result |= (int)(x.Value as object));

            return GetInstances().First().GetValue(result);
        }

        /// <summary>
        /// Used Internally for Flag implementations to convert an integer to TDerived by the derived class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>int.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual TDerived GetValue(int value) => throw new NotImplementedException();

        private static TDerived GetFlagDefault(TValue value)
        {
            if (!IsFlag)
            {
                return null;
            }
            EnsureSetup();
            var d = (TDerived) defaultDerived.MemberwiseClone();
            d.valueAttribute = new ValueAttribute(value);

            return d;
        }

        #endregion Methods
    }
}
