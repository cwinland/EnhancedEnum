// ***********************************************************************
// Assembly         : EnhancedEnum.Tests
// Author           : chris
// Created          : 11-15-2020
//
// Last Modified By : chris
// Last Modified On : 11-16-2020
// ***********************************************************************
// <copyright file="FlagsEnum.cs" company="EnhancedEnum.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using EnhancedEnum.Attributes;

namespace EnhancedEnum.Tests.Enums
{
    /// <summary>
    /// Class FlagsEnum. This class cannot be inherited.
    /// Implements the <see cref="EnhancedEnum{TValue,TDerived}" />
    /// </summary>
    /// <seealso cref="EnhancedEnum{TValue,TDerived}" />
    [Flag]
    public sealed class FlagsEnum : EnhancedEnum<int, FlagsEnum>
    {
        #region Fields

        /// <summary>
        /// The none
        /// </summary>
        [Value(0)]
        public static readonly FlagsEnum None = new FlagsEnum();

        /// <summary>
        /// The one
        /// </summary>
        [Value(1)]
        public static readonly FlagsEnum One = new FlagsEnum();

        /// <summary>
        /// The two
        /// </summary>
        [Value(2)]
        public static readonly FlagsEnum Two = new FlagsEnum();

        /// <summary>
        /// The four
        /// </summary>
        [Value(4)]
        public static readonly FlagsEnum Four = new FlagsEnum();

        /// <summary>
        /// The eight
        /// </summary>
        [Value(8)]
        public static readonly FlagsEnum Eight = new FlagsEnum();

        #endregion

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String" /> to <see cref="FlagsEnum" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator FlagsEnum(string value) => Convert(value);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32" /> to <see cref="FlagsEnum" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator FlagsEnum(int value) => Convert(value);

        /// <inheritdoc />
        protected override FlagsEnum GetValue(int value) => Convert(value);
    }
}
