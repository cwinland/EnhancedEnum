// ***********************************************************************
// Assembly         : EnhancedEnum Author : chris Created : 11-13-2020
// Author           : chris Created : 11-13-2020
// Created          : 11-13-2020
//
// Last Modified By : chris Last Modified On : 11-13-2020 ***********************************************************************
// Last Modified On : 11-16-2020
// ***********************************************************************
// <copyright file="IEnhancedEnum.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using EnhancedEnum.Attributes;

namespace EnhancedEnum
{
    /// <summary>
    /// Interface IEnhancedEnum Implements the <see cref="System.IEquatable{TDerived}" /> Implements the <see cref="System.IComparable{TDerived}" />
    /// Implements the <see cref="System.IComparable" /> Implements the <see cref="System.Collections.Generic.IComparer{TDerived}" />
    /// </summary>
    /// <typeparam name="TValue">The type of the t value.</typeparam>
    /// <typeparam name="TDerived">The type of the t derived.</typeparam>
    /// <seealso cref="System.IEquatable{TDerived}" />
    /// <seealso cref="System.IComparable{TDerived}" />
    /// <seealso cref="System.IComparable" />
    /// <seealso cref="System.Collections.Generic.IComparer{TDerived}" />
    public interface IEnhancedEnum<out TValue, TDerived> : IEquatable<TDerived>,
                                                           IComparable<TDerived>,
                                                           IComparable, IComparer<TDerived>

    {
        /// <summary>
        /// Gets the description using the associated <see cref="DescriptionAttribute" />.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; }

        /// <summary>
        /// Gets the name using the associated <see cref="DisplayNameAttribute" />.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the value using the associated <see cref="ValueAttribute" />.
        /// </summary>
        /// <value>The value.</value>
        TValue Value { get; }

        /// <summary>
        /// Determines whether the specified flag has flag.
        /// </summary>
        /// <param name="flag">The flag.</param>
        /// <returns><c>true</c> if the specified flag has flag; otherwise, <c>false</c>.</returns>
        bool HasFlag(int flag);

    }
}
