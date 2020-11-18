// ***********************************************************************
// Assembly         : EnhancedEnum.Tests
// Author           : chris
// Created          : 11-15-2020
//
// Last Modified By : chris
// Last Modified On : 11-15-2020
// ***********************************************************************
// <copyright file="BadEnum.cs" company="EnhancedEnum.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using EnhancedEnum.Attributes;

namespace EnhancedEnum.Tests.Enums
{
    /// <summary>
    /// Class BadEnum.
    /// Implements the <see cref="EnhancedEnum{TValue,TDerived}" />
    /// </summary>
    /// <seealso cref="EnhancedEnum{TValue,TDerived}" />
    public class BadEnum : EnhancedEnum<int, BadEnum>
    {
        /// <summary>
        /// The wrong.
        /// </summary>
        [Value("WrongValue Type")]
        public static readonly BadEnum Wrong = new BadEnum();

        [Value(1)]
        public static readonly BadEnum Value1 = new BadEnum();

        [Value(1)]
        public static readonly BadEnum Value1Duplicate = new BadEnum();

        /// <summary>
        /// The test field.
        /// </summary>
        public static readonly string TestField = "";
    }
}
