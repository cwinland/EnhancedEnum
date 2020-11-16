// ***********************************************************************
// Assembly         : EnhancedEnum.Tests
// Author           : chris
// Created          : 11-15-2020
//
// Last Modified By : chris
// Last Modified On : 11-15-2020
// ***********************************************************************
// <copyright file="NonIntTests.cs" company="EnhancedEnum.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using EnhancedEnum.Tests.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnhancedEnum.Tests
{
    /// <summary>
    /// Defines test class NonIntTests.
    /// </summary>
    [TestClass]
    public class NonIntTests
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Init() { }
        /// <summary>
        /// Defines the test method NonIntEnum_Tests.
        /// </summary>
        [TestMethod]
        public void NonIntEnum_Tests()
        {
            var nie = NonIntEnum.December;
            var nie2 = NonIntEnum.January;
            var nie3 = NonIntEnum.November;

            DateTime d1 = NonIntEnum.December;
            string s1 = NonIntEnum.December;
            NonIntEnum nie4 = "December 1st";
            NonIntEnum nie5 = DateTime.Parse("12/1/2020");

            nie.Should()
               .Be("December 1st");

            nie2.Should()
                .Be("January");

            nie3.Should()
                .Be(new DateTime(2020, 11, 1));

            d1.Should()
              .Be(NonIntEnum.December);
            s1.Should()
              .Be(NonIntEnum.December);
            nie4.Should()
                .Be(NonIntEnum.December);
            nie5.Should()
                .Be(NonIntEnum.December);
        }

        /// <summary>
        /// Defines the test method Try_Tests.
        /// </summary>
        [TestMethod]
        public void Try_Tests()
        {
            NonIntEnum.TryConvert("December 1st", out _)
                      .Should()
                      .BeTrue();
            NonIntEnum.TryConvert("December 2nd", out _)
                      .Should()
                      .BeFalse();
            NonIntEnum.TryConvert(DateTime.Parse("11/1/2020"), out _)
                      .Should()
                      .BeTrue();
            NonIntEnum.TryConvert(DateTime.Parse("11/2/2020"), out _)
                      .Should()
                      .BeFalse();
        }

        /// <summary>
        /// Defines the test method Compare_Tests.
        /// </summary>
        [TestMethod]
        public void Compare_Tests()
        {
            var dec = NonIntEnum.December;
            var jan = NonIntEnum.January;
            dec.Equals(jan)
               .Should()
               .BeFalse();

            var vals = new List<NonIntEnum>()
            {
                NonIntEnum.December, NonIntEnum.January, NonIntEnum.November,
            };
            vals.OrderBy(x => x.Value).Should().HaveCount(3);
            vals.OrderBy(x => x.Name).Should().HaveCount(3);

            string dec1 = NonIntEnum.December;
            string dec2 = NonIntEnum.December;
            string jan1 = NonIntEnum.January;

            string.Compare(dec1, jan1, StringComparison.Ordinal).Should().NotBe(0);
            string.Compare(dec1, dec2, StringComparison.CurrentCultureIgnoreCase).Should().Be(0);
        }
    }
}
