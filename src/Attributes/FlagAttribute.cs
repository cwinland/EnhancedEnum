// ***********************************************************************
// Assembly         : EnhancedEnum
// Author           : chris
// Created          : 11-14-2020
//
// Last Modified By : chris
// Last Modified On : 11-16-2020
// ***********************************************************************
// <copyright file="FlagAttribute.cs" company="Microsoft Corporation">
//     copyright(c) 2020 Christopher Winland
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace EnhancedEnum.Attributes
{
    /// <summary>
    /// Class FlagAttribute. This class cannot be inherited.
    /// Implements the <see cref="System.Attribute" />
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class FlagAttribute : Attribute { }
}
