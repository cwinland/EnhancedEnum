using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EnhancedEnum
{
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
        ///   Count of enumerations.
        /// </summary>
        public static int Count => values.Count;

        /// <summary>
        ///   Enumerable list of all possible values.
        /// </summary>
        public static IEnumerable<TDerived> Values => values.Values;

        /// <summary>
        ///   Description of enumeration as described in <see cref="DescriptionAttribute" />
        /// </summary>
        public string Description
        {
            get
            {
                EnsureSetup();

                return descriptionAttribute?.Description ?? name;
            }
        }

        /// <summary>
        ///   Name of enumeration as described in <see cref="DisplayNameAttribute" />
        /// </summary>
        public string Name
        {
            get
            {
                EnsureSetup();
                return displayNameAttribute?.Name ?? name;
            }
        }

        /// <summary>
        ///   Value of enumeration as described in <see cref="ValueAttribute" />
        /// </summary>
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

        public static TDerived Convert(string value) => Values.FirstOrDefault(x => x.Name == value);

        public static TDerived Convert(TValue value) => Values.FirstOrDefault(x => x.Value.Equals(value));

        public static implicit operator EnhancedEnum<TValue, TDerived>(string value) =>
                            Values.FirstOrDefault(x => x.Name == value);

        public static implicit operator EnhancedEnum<TValue, TDerived>(TValue value2) =>
            Values.FirstOrDefault(x => x.Value.Equals(value2));

        public static implicit operator string(EnhancedEnum<TValue, TDerived> value) => value.Name;

        public static implicit operator TValue(EnhancedEnum<TValue, TDerived> value2) => value2.Value;

        public static bool operator !=(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => !(left == right);

        public static bool operator <(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) =>
            left.Value == null ? right.Value is object : left.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) =>
            left.Value == null || left.Value.CompareTo(right.Value) <= 0;

        public static bool operator ==(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) =>
            left?.Equals(right) ?? right is null;

        public static bool operator >(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) =>
            left.Value is object && left.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) =>
            left.Value == null ? right.Value == null : left.Value.CompareTo(right.Value) >= 0;

        public static bool TryConvert(string value, out TDerived result)
        {
            result = Values.FirstOrDefault(x => x.Name == value);

            return result != null;
        }

        public static bool TryConvert(TValue value, out TDerived result)
        {
            result = Values.FirstOrDefault(x => x.Value.Equals(value));
            return result != null;
        }

        int IComparer<TDerived>.Compare(TDerived x, TDerived y) =>
                            x == null
                ? -1
                : y == null
                    ? 1
                    : string.Compare(x.Name, y.Name, StringComparison.Ordinal);

        int IComparable<TDerived>.CompareTo(TDerived other) => Value.CompareTo(other.Value);

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

        bool IEquatable<TDerived>.Equals(TDerived other) => other is { } && Name.Equals(other.Name);

        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString() => Name;

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
