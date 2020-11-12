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

        private static bool isInitialized;
        private static SortedList<TValue, TDerived> values;
        private DescriptionAttribute descriptionAttribute;
        private string name;

        #endregion Fields

        #region Constructors

        protected EnhancedEnum(TValue value)
        {
            values ??= new SortedList<TValue, TDerived>();
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

                return this.descriptionAttribute != null ? this.descriptionAttribute.Description : this.name;
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

        /// <summary>
        ///   The nameValue of the enum item
        /// </summary>
        public string NameValue => this.Name;

        public TValue Value { get; }

        #endregion Properties

        #region Methods

        public static implicit operator EnhancedEnum<TValue, TDerived>(string value) => values.Values.FirstOrDefault(x => x.Name == value);

        public static implicit operator EnhancedEnum<TValue, TDerived>(TValue value2) => values[value2];

        public static implicit operator string(EnhancedEnum<TValue, TDerived> value) => value.NameValue;

        public static implicit operator TValue(EnhancedEnum<TValue, TDerived> value2) => value2.Value;

        public static TDerived Parse(string name) =>
            values.Values.FirstOrDefault(x => x.Name == name);

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
                    : x.NameValue.CompareTo(y.NameValue);

        int IComparable<TDerived>.CompareTo(TDerived other) => this.Value.CompareTo(other.Value);

        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
            {
                return -1;
            }

            if (obj is string value)
            {
                return this.NameValue.CompareTo(value);
            }

            if (obj is TDerived @enum)
            {
                return this.NameValue.CompareTo(@enum.NameValue);
            }
            return -1;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj switch
            {
                string value => this.NameValue.Equals(value),
                TDerived @enum => this.NameValue.Equals(@enum.NameValue),
                _ => false,
            };
        }

        bool IEquatable<TDerived>.Equals(TDerived other) => other is { } && this.NameValue.Equals(other.NameValue);

        public override int GetHashCode() => this.NameValue.GetHashCode();

        public override string ToString() => this.Name;

        protected static TDerived Convert(string value) => values.Values.FirstOrDefault(x => x.Name == value);

        protected static TDerived Convert(TValue value) => values[value];

        private static void CheckInitialized()
        {
            if (isInitialized)
            {
                return;
            }

            //var resources = new ResourceManager(typeof(TDerived).Name, typeof(TDerived).Assembly);

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

            isInitialized = true;
        }

        public static bool operator ==(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => left?.Equals(right) ?? right is null;

        public static bool operator !=(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => !(left == right);

        public static bool operator <(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => left.Value == null ? right.Value is object : left.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => left.Value == null || left.Value.CompareTo(right.Value) <= 0;

        public static bool operator >(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => left.Value is object && left.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(EnhancedEnum<TValue, TDerived> left, EnhancedEnum<TValue, TDerived> right) => left.Value == null ? right.Value == null : left.Value.CompareTo(right.Value) >= 0;

        #endregion Methods
    }
}
