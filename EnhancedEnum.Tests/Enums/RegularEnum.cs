// ***********************************************************************
// Assembly         : EnhancedEnum.Tests
// Author           : chris
// Created          : 11-16-2020
//
// Last Modified By : chris
// Last Modified On : 11-16-2020
// ***********************************************************************
// <copyright file="RegularEnum.cs" company="EnhancedEnum.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace EnhancedEnum.Tests.Enums
{
    /// <summary>
    /// Enum RegularEnum
    /// </summary>
    [Flags]
    public enum RegularEnum
    {
        /// <summary>
        /// The running
        /// </summary>
        Running = 1,

        /// <summary>
        /// The stopped
        /// </summary>
        Stopped = 2,

        /// <summary>
        /// The error
        /// </summary>
        Error = 4,
    }
}
