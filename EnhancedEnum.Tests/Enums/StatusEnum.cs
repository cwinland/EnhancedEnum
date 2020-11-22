// ***********************************************************************
// Assembly         : EnhancedEnum.Tests
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-15-2020
// ***********************************************************************
// <copyright file="StatusEnum.cs" company="EnhancedEnum.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using EnhancedEnum.Attributes;

namespace EnhancedEnum.Tests.Enums
{
    /// <summary>
    /// Class StatusTest. This class cannot be inherited. Implements the <see cref="EnhancedEnum{TValue,TDerived}" />
    /// </summary>
    /// <seealso cref="EnhancedEnum{TValue,TDerived}" />
    public sealed class StatusEnum : EnhancedEnum<int, StatusEnum>
    {
        #region Fields

        // DisplayName is 'In Process' (because of the DisplayName attribute). Value is 5 (because of the Value attribute).
        /// <summary>
        /// The running
        /// </summary>
        [Description("Indicates Running")]
        [DisplayName("In Process")]
        [Value(5)]
        public static readonly StatusEnum Running = new StatusEnum();

        // DisplayName is Stopped. Value is 2 (because it is the second one in the list).
        /// <summary>
        /// The stopped
        /// </summary>
        public static readonly StatusEnum Stopped = new StatusEnum();

        // DisplayName is Error. Value is 3 (because it is the third one in the list).
        /// <summary>
        /// The error
        /// </summary>
        public static readonly StatusEnum Error = new StatusEnum();

        #endregion

        // This allows us to assign back to this class.
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String" /> to <see cref="StatusEnum" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator StatusEnum(string value) => Convert(value);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32" /> to <see cref="StatusEnum" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator StatusEnum(int value) => Convert(value);
    }
}
