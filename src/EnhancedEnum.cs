using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace EnhancedEnum
{
    public abstract class EnhancedEnum<TValue, TDerived>
                : IEquatable<TDerived>,
                  IComparable<TDerived>,
                  IComparable, IComparer<TDerived>
        where TValue : IComparable<TValue>, IEquatable<TValue>
        where TDerived : EnhancedEnum<TValue, TDerived>
    {
        #region Fields

        private static bool isSetup;
        private static SortedList<TValue, TDerived> values;
        private DescriptionAttribute descriptionAttribute;
        private string name;

        #endregion Fields

        #region Constructors

        protected EnhancedEnum(TValue value)
        {
            values ??= new SortedList<TValue, TDerived>();

            if (values.ContainsKey(value))
            {
                throw new ArgumentException("Value already exists. Value parameter must be unique.", nameof(value));
            }
            this.Value = value;
            values.Add(value, (TDerived)this);
        }

        #endregion Constructors

        #region Properties

        public static int Count => values.Count;

        /// <summary>
        ///   Enumerable list of all possible values.
        /// </summary>
        public static IEnumerable<TDerived> Values => values.Values;

        public string Description
        {
            get
            {
                CheckInitialized();

                return this.descriptionAttribute != null
                    ? this.descriptionAttribute.Description
                    : this.name;
            }
        }

        public string Name
        {
            get
            {
                CheckInitialized();
                return this.name;
            }
        }

        public TValue Value { get; }

        #endregion Properties

        #region Methods

        public static implicit operator EnhancedEnum<TValue, TDerived>(string value) => values.Values.FirstOrDefault(x => x.Name == value);

        public static implicit operator EnhancedEnum<TValue, TDerived>(TValue value2) => values[value2];

        public static implicit operator string(EnhancedEnum<TValue, TDerived> value) => value.Name;

        public static implicit operator TValue(EnhancedEnum<TValue, TDerived> value2) => value2.Value;

        public static bool operator !=(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => !(left == right);

        public static bool operator <(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => left.Value == null ? right.Value is object : left.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => left.Value == null || left.Value.CompareTo(right.Value) <= 0;

        public static bool operator ==(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => left?.Equals(right) ?? right is null;

        public static bool operator >(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => left.Value is object && left.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => left.Value == null ? right.Value == null : left.Value.CompareTo(right.Value) >= 0;

        public static TDerived Parse(string name) => values.Values.FirstOrDefault(x => x.Name == name);

        public static TDerived Parse(TValue intValue) =>
            values.Values.FirstOrDefault(value => value.Value.Equals(intValue));

        public static bool TryConvert(string value, out TDerived result)
        {
            result = values.Values.FirstOrDefault(x => x.Name == value);

            return result != null;
        }

        public static bool TryConvert(TValue value, out TDerived result) => values.TryGetValue(value, out result);

        int IComparer<TDerived>.Compare(TDerived x, TDerived y) =>
            x == null
                ? -1
                : y == null
                    ? 1
                    : string.Compare(x.Name, y.Name, StringComparison.Ordinal);

        int IComparable<TDerived>.CompareTo(TDerived other) => this.Value.CompareTo(other.Value);

        int IComparable.CompareTo(object obj)
        {
            return obj switch
            {
                null => -1,
                string value => string.Compare(this.Name, value, StringComparison.Ordinal),
                TDerived @enum => string.Compare(this.Name, @enum.Name, StringComparison.Ordinal),
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
                string value => this.Name.Equals(value),
                TDerived @enum => this.Name.Equals(@enum.Name),
                _ => false,
            };
        }

        bool IEquatable<TDerived>.Equals(TDerived other) => other is { } && this.Name.Equals(other.Name);

        public override int GetHashCode() => this.Name.GetHashCode();

        public override string ToString() => this.Name;

        protected static TDerived Convert(string value) => values.Values.FirstOrDefault(x => x.Name == value);

        protected static TDerived Convert(TValue value) => values[value];

        private static void CheckInitialized()
        {
            if (isSetup)
            {
                return;
            }

            var fields = typeof(TDerived)
                         .GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public)
                         .Where(t => t.FieldType == typeof(TDerived));

            foreach (var field in fields)
            {
                var instance = (TDerived)field.GetValue(null);

                if (instance == null)
                {
                    continue;
                }

                instance.name = field.Name;
                instance.descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();
            }

            isSetup = true;
        }

        #endregion Methods
    }
}
