using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace EnhancedEnum
{
    public abstract class EnhancedEnum<TValue, TValue2, TDerived>
                : IEquatable<TDerived>,
                  IComparable<TDerived>,
                  IComparable, IComparer<TDerived>
        where TValue : IComparable<TValue>, IEquatable<TValue>
        where TValue2 : IComparable<TValue2>, IEquatable<TValue2>
        where TDerived : EnhancedEnum<TValue, TValue2, TDerived>
    {
        #region Fields

        private static bool isInitialized;
        private static SortedList<TValue, TDerived> stringValues;
        private static SortedList<TValue2, TDerived> intValues;
        private DescriptionAttribute descriptionAttribute;
        private string name;

        #endregion Fields

        #region Constructors

        protected EnhancedEnum(TValue stringValue, TValue2 intValue)
        {
            stringValues ??= new SortedList<TValue, TDerived>();
            this.StringValue = stringValue;
            stringValues.Add(stringValue, (TDerived)this);

            intValues ??= new SortedList<TValue2, TDerived>();
            this.Value = intValue;
            intValues.Add(intValue, (TDerived)this);
        }

        #endregion Constructors

        #region Properties

        public static int Count => stringValues.Count;

        public static IEnumerable<TDerived> Values => stringValues.Values;

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
        ///   The stringValue of the enum item
        /// </summary>
        public TValue StringValue { get; }

        public TValue2 Value { get; }

        #endregion Properties

        #region Methods

        public static implicit operator EnhancedEnum<TValue, TValue2, TDerived>(TValue value) => stringValues[value];

        public static implicit operator EnhancedEnum<TValue, TValue2, TDerived>(TValue2 value2) => intValues[value2];

        public static implicit operator TValue(EnhancedEnum<TValue, TValue2, TDerived> value) => value.StringValue;

        public static implicit operator TValue2(EnhancedEnum<TValue, TValue2, TDerived> value2) => value2.Value;

        public static TDerived Parse(TValue name) =>
            stringValues.Values.FirstOrDefault(value => value.Equals(name));

        public static TDerived Parse(TValue2 intValue) =>
            stringValues.Values.FirstOrDefault(value => value.Value.Equals(intValue));

        public static bool TryConvert(TValue value, out TDerived result) => stringValues.TryGetValue(value, out result);

        public static bool TryConvert(TValue2 value, out TDerived result) => intValues.TryGetValue(value, out result);

        int IComparer<TDerived>.Compare(TDerived x, TDerived y) =>
            x == null
                ? -1
                : y == null
                    ? 1
                    : x.StringValue.CompareTo(y.StringValue);

        int IComparable<TDerived>.CompareTo(TDerived other) => this.Value.CompareTo(other.Value);

        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
            {
                return -1;
            }

            if (obj is TValue value)
            {
                return this.StringValue.CompareTo(value);
            }

            if (obj is TDerived @enum)
            {
                return this.StringValue.CompareTo(@enum.StringValue);
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
                TValue value => this.StringValue.Equals(value),
                TDerived @enum => this.StringValue.Equals(@enum.StringValue),
                _ => false,
            };
        }

        bool IEquatable<TDerived>.Equals(TDerived other) => other is { } && this.StringValue.Equals(other.StringValue);

        public override int GetHashCode() => this.StringValue.GetHashCode();

        public override string ToString() => this.StringValue.ToString();

        protected static TDerived Convert(TValue value) => stringValues[value];

        protected static TDerived Convert(TValue2 value) => intValues[value];

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

        public static bool operator ==(EnhancedEnum<TValue, TValue2, TDerived> left, EnhancedEnum<TValue, TValue2, TDerived> right) => left?.Equals(right) ?? right is null;

        public static bool operator !=(EnhancedEnum<TValue, TValue2, TDerived> left, EnhancedEnum<TValue, TValue2, TDerived> right) => !(left == right);

        public static bool operator <(EnhancedEnum<TValue, TValue2, TDerived> left, EnhancedEnum<TValue, TValue2, TDerived> right) => left.Value == null ? right.Value is object : left.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(EnhancedEnum<TValue, TValue2, TDerived> left, EnhancedEnum<TValue, TValue2, TDerived> right) => left.Value == null || left.Value.CompareTo(right.Value) <= 0;

        public static bool operator >(EnhancedEnum<TValue, TValue2, TDerived> left, EnhancedEnum<TValue, TValue2, TDerived> right) => left.Value is object && left.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(EnhancedEnum<TValue, TValue2, TDerived> left, EnhancedEnum<TValue, TValue2, TDerived> right) => left.Value == null ? right.Value == null : left.Value.CompareTo(right.Value) >= 0;

        #endregion Methods
    }
}
