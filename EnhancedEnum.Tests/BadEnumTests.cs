// ***********************************************************************
// Assembly         : EnhancedEnum.Tests
// Author           : chris
// Created          : 11-15-2020
//
// Last Modified By : chris
// Last Modified On : 11-15-2020
// ***********************************************************************
// <copyright file="BadEnumTests.cs" company="EnhancedEnum.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using EnhancedEnum.Tests.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EnhancedEnum.Tests
{
    /// <summary>
    /// Defines test class BadEnumTests.
    /// </summary>
    [TestClass]
    public class BadEnumTests
    {
        /// <summary>
        /// Defines the test method Test_BadValue.
        /// </summary>
        [TestMethod]
        public void Test_BadValue()
        {
            var t = BadEnum.Wrong;
            Func<int> act = () => t.Value;
            Func<string> act2 = () => t.Description;
            Func<string> act3 = () => t.Name;
            act.Should()
               .Throw<ArgumentException>();
            act2.Should()
               .Throw<ArgumentException>();
            act3.Should()
                .Throw<ArgumentException>();
        }
    }
}
