// ***********************************************************************
// Assembly         : EnhancedEnum Author : chris Created : 11-13-2020
// Author           : chris Created : 11-13-2020
// Created          : 11-13-2020
//
// Last Modified By : chris Last Modified On : 11-13-2020 ***********************************************************************
// Last Modified On : 11-14-2020
// ***********************************************************************
// <copyright file="DisplayNameAttribute.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace EnhancedEnum.Attributes
{
    /// <summary>
    /// Class DisplayNameAttribute. This class cannot be inherited. Implements the <see cref="System.Attribute" />
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DisplayNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayNameAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public DisplayNameAttribute(string name) => Name = name;
    }
}
