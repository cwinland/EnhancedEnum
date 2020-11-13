// ***********************************************************************
// Assembly         : EnhancedEnum.Tests
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-13-2020
// ***********************************************************************
// <copyright file="StatusTest.cs" company="EnhancedEnum.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace EnhancedEnum.Tests
{
    /// <summary>
    /// Class StatusTest. This class cannot be inherited. Implements the <see cref="EnhancedEnum.EnhancedEnum{System.Int32,&#xD;&#xA;EnhancedEnum.Tests.StatusTest}" />
    /// </summary>
    /// <seealso cref="EnhancedEnum.EnhancedEnum{System.Int32, EnhancedEnum.Tests.StatusTest}" />
    public sealed class StatusTest : EnhancedEnum<int, StatusTest>
    {
        // DisplayName is 'In Process' (because of the DisplayName attribute). Value is 5 (because of the Value attribute).
        /// <summary>
        /// The running
        /// </summary>
        [Description("Indicates Running")]
        [DisplayName("In Process")]
        [Value(5)]
        public static readonly StatusTest Running = new StatusTest();

        // DisplayName is Stopped. Value is 2 (because it is the second one in the list).
        /// <summary>
        /// The stopped
        /// </summary>
        public static readonly StatusTest Stopped = new StatusTest();

        // DisplayName is Error. Value is 3 (because it is the third one in the list).
        /// <summary>
        /// The error
        /// </summary>
        public static readonly StatusTest Error = new StatusTest();

        // This allows us to assign back to this class.

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="StatusTest"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator StatusTest(string value) => Convert(value);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32" /> to <see cref="StatusTest" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator StatusTest(int value) => Convert(value);
    }
}
