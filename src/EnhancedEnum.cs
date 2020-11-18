// ***********************************************************************
// Assembly         : EnhancedEnum
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-17-2020
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

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once NotAccessedField.Global
        /// <summary>
        /// The description attribute
        /// </summary>
        internal DescriptionAttribute descriptionAttribute;

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once NotAccessedField.Global
        /// <summary>
        /// The display name attribute
        /// </summary>
        internal DisplayNameAttribute displayNameAttribute;

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once NotAccessedField.Global
        /// <summary>
        /// The value attribute
        /// </summary>
        internal ValueAttribute valueAttribute;

        private static TDerived defaultDerived;
        // ReSharper disable once StaticMemberInGenericType
        private static bool isSetup;
        private static SortedList<int, TDerived> values;
        private string name;
        private readonly List<TValue> validationList = new List<TValue>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedEnum{TValue, TDerived}" /> class.
        /// </summary>
        /// <exception cref="ArgumentException">Provided value, '{}' must be of type 'TValue'. Do you need to overload TypeConverter in the derived class?</exception>
        /// <exception cref="ArgumentException">Provided value, '{}' must be unique.</exception>
        protected EnhancedEnum()
        {
            values ??= new SortedList<int, TDerived>();
            ValidateAttributes();
            var autoValue = values.Count + 1;
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
        /// Gets a value indicating whether this instance is flag.
        /// </summary>
        /// <value><c>true</c> if this instance is flag; otherwise, <c>false</c>.</value>
        public static bool IsFlag =>
            typeof(TDerived)
                .GetCustomAttribute<FlagAttribute>() !=
            null;

        /// <summary>
        /// Gets or sets a value indicating whether to throw an Exception (instead of null value) when a conversion error occurs.
        /// </summary>
        /// <value><c>true</c> if [throw on error]; otherwise, <c>false</c>.</value>
        // ReSharper disable once StaticMemberInGenericType
        public static bool ThrowOnError { get; set; }

        /// <summary>
        /// Enumerable list of all possible values.
        /// </summary>
        /// <value>The values.</value>
        public static List<TDerived> Values => values.Values.ToList();

        /// <summary>
        /// Description of enumeration as described in <see cref="DescriptionAttribute" />
        /// </summary>
        /// <value>The description.</value>
        public string Description => GetAttribute<DescriptionAttribute>()?.Description ?? Name;

        /// <summary>
        /// Name of enumeration as described in <see cref="DisplayNameAttribute" />
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return IsFlag ? string.Join(",",
                                            values.Where(x => (int)(x.Value.Value as object) > 0 && HasFlag((int)(x.Value.Value as object)))
                                                  .Select(x => x.Value.GetAttribute<DisplayNameAttribute>()?.Name ?? x.Value.name)
                                                  .ToList())
                    : GetAttribute<DisplayNameAttribute>()?.Name ?? name;
            }
        }

        /// <summary>
        /// Value of enumeration as described in <see cref="ValueAttribute" />
        /// </summary>
        /// <value>The <see cref="valueAttribute" /> or default value.</value>
        /// <exception cref="ArgumentException">Invalid Value. - TValue</exception>
        public TValue Value
        {
            get
            {
                var attributeValue = GetAttribute<ValueAttribute>();

                if (attributeValue is null)
                {
                    throw new ArgumentException("Invalid Value.", nameof(TValue));
                }

                var convertedType = TypeConverter(attributeValue.Value);

                return convertedType.Equals(default)
                    ? (TValue) attributeValue
                          .Value ??
                      default
                    : convertedType;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>TDerived.</returns>
        public static TDerived Convert(string value)
        {
            return ThrowOnError
                ? Values.First(x => (x.GetAttribute<DisplayNameAttribute>()?.Name ?? x.name) == value) ?? GetFlagDefault(value)
                : Values.FirstOrDefault(x => (x.GetAttribute<DisplayNameAttribute>()?.Name ?? x.name) == value) ??
                  GetFlagDefault(value);
        }

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

        /// <inheritdoc />
        int IComparer<TDerived>.Compare(TDerived x, TDerived y) =>
                            x == null
                ? -1
                : y == null
                    ? 1
                    : string.Compare(x.Name, y.Name, StringComparison.Ordinal);

        /// <inheritdoc />
        int IComparable<TDerived>.CompareTo(TDerived other) => other == null ? -1 : Value.CompareTo(other.Value);

        /// <inheritdoc />
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
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString() => Name;

        /// <summary>
        /// Used Internally for Flag implementations to convert an integer to TDerived by the derived class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>int.</returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual TDerived GetValue(int value) => throw new NotImplementedException();

        /// <summary>
        /// Types the converter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>TValue.</returns>
        protected virtual TValue TypeConverter(object value) => (TValue)value;

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
            typeof(TDerived)
                .GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public)
                .Where(t => t.FieldType == typeof(TDerived) && (TDerived)t.GetValue(null) != null)
                .ToList()
                .ForEach(
                    field =>
                    {
                        var instance = (TDerived)field.GetValue(null);

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

            list.ForEach(x => result |= (int)(x.Value as object));

            return GetInstances().First().GetValue(result);
        }

        private static TDerived GetFlagDefault(TValue value)
        {
            if (!IsFlag)
            {
                return null;
            }
            EnsureSetup();
            var d = (TDerived)defaultDerived.MemberwiseClone();
            d.valueAttribute = new ValueAttribute(value);

            return d;
        }

        private static IEnumerable<TDerived> GetInstances() =>
            typeof(TDerived)
                .GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public)
                .Where(t => t.FieldType == typeof(TDerived) && (TDerived)t.GetValue(null) != null)
                .ToList()
                .Select(field => (TDerived)field.GetValue(null));

        /// <summary>
        /// Designed to get the Attribute fields by Attribute type while ensuring they are populated correctly before accessing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><see cref="Attribute"/></returns>
        private T GetAttribute<T>() where T : Attribute
        {
            EnsureSetup(); // Must be called before accessing any attribute value.

            var attributes = GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            var attribute = attributes.First(x => x.FieldType == typeof(T));

            return (T)attribute.GetValue(this);
        }

        private void ValidateAttributes()
        {
            GetType()
                .GetRuntimeFields()
                .Where(x => x.GetCustomAttribute<ValueAttribute>() != null)
                .Select(x => x.GetCustomAttribute<ValueAttribute>())
                .ToList()
                .ForEach(
                    desiredValue =>
                    {
                        TValue convertedValue = default;

                        try
                        {
                            if (desiredValue != null)
                            {
                                convertedValue = TypeConverter(desiredValue.Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException(
                                $"Provided value, '{desiredValue?.Value}' must be of type 'TValue'. Do you need to overload TypeConverter in the derived class?",
                                nameof(ValueAttribute),
                                ex
                            );
                        }

                        if (desiredValue == null)
                        {
                            return;
                        }

                        if (convertedValue != null &&
                            !validationList.Contains(convertedValue))
                        {
                            validationList.Add(convertedValue);
                        }
                        else
                        {
                            if (convertedValue != null && !convertedValue.Equals(default))
                            {
                                throw new ArgumentException(
                                    $"Provided value, '{convertedValue}' must be unique.",
                                    nameof(ValueAttribute)
                                );
                            }
                        }
                    }
                );
        }

        #endregion Methods
    }
}
