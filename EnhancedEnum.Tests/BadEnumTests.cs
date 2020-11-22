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
using System;
using EnhancedEnum.Tests.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Func<BadEnum> act = () => BadEnum.Wrong;
            act.Should()
               .Throw<TypeInitializationException>()
               .WithInnerException<ArgumentException>();

            Func<BadEnum> act2 = () => BadEnum.Value1;
            act2.Should()
                .Throw<TypeInitializationException>()
                .WithInnerException<ArgumentException>();

            Func<BadEnum> act3 = () => BadEnum.Value1Duplicate;
            act3.Should()
                .Throw<TypeInitializationException>()
                .WithInnerException<ArgumentException>();
        }
    }
}
