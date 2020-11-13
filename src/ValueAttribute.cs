// ***********************************************************************
// Assembly         : EnhancedEnum Author : chris Created : 11-13-2020
// Author           : chris Created : 11-13-2020
// Created          : 11-13-2020
//
// Last Modified By : chris Last Modified On : 11-13-2020 ***********************************************************************
// Last Modified On : 11-13-2020
// ***********************************************************************
// <copyright file="ValueAttribute.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace EnhancedEnum
{
    /// <summary>
    /// Class ValueAttribute. This class cannot be inherited. Implements the <see cref="System.Attribute" />
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ValueAttribute : Attribute
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAttribute" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ValueAttribute(object value) => Value = value;
    }
}
