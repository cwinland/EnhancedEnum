// ***********************************************************************
// Assembly         : EnhancedEnum.Tests
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-16-2020
// ***********************************************************************
// <copyright file="NonIntEnum.cs" company="EnhancedEnum.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using EnhancedEnum.Attributes;
using System;

namespace EnhancedEnum.Tests.Enums
{
    /// <summary>
    /// Class NonInt. This class cannot be inherited. Implements the <see cref="EnhancedEnum{TValue,TDerived}" />
    /// </summary>
    /// <seealso cref="EnhancedEnum{TValue,TDerived}" />
    public sealed class NonIntEnum : EnhancedEnum<DateTime, NonIntEnum>
    {
        /// <summary>
        /// December
        /// </summary>
        [Description("Indicates Month of December")]
        [DisplayName("December 1st")]
        [Value("12/1/2020")]
        public static readonly NonIntEnum December = new NonIntEnum();

        // DisplayName is Stopped. Value is 2 (because it is the second one in the list).
        /// <summary>
        /// November
        /// </summary>
        [Value("11/1/2020")]
        public static readonly NonIntEnum November = new NonIntEnum();

        /// <summary>
        /// January
        /// </summary>
        [Value("1/1/2020")]
        public static readonly NonIntEnum January = new NonIntEnum();

        // This allows us to assign back to this class.
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String" /> to <see cref="NonIntEnum" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator NonIntEnum(string value) => Convert(value);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32" /> to <see cref="NonIntEnum" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator NonIntEnum(DateTime value) => Convert(value);

        /// <inheritdoc />
        protected override DateTime TypeConverter(object value) => DateTime.Parse(value.ToString());
    }
}
