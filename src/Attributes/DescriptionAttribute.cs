// ***********************************************************************
// Assembly         : EnhancedEnum Author : chris Created : 11-13-2020
// Author           : chris Created : 11-13-2020
// Created          : 11-13-2020
//
// Last Modified By : chris Last Modified On : 11-13-2020 ***********************************************************************
// Last Modified On : 11-15-2020
// ***********************************************************************
// <copyright file="DescriptionAttribute.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace EnhancedEnum.Attributes
{
    /// <summary>
    /// Class DescriptionAttribute. This class cannot be inherited. Implements the <see cref="System.Attribute" />
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionAttribute" /> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public DescriptionAttribute(string description) => Description = description;
    }
}
