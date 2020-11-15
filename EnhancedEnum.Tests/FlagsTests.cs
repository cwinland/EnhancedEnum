// ***********************************************************************
// Assembly         : EnhancedEnum.Tests
// Author           : chris
// Created          : 11-14-2020
//
// Last Modified By : chris
// Last Modified On : 11-15-2020
// ***********************************************************************
// <copyright file="FlagsTests.cs" company="EnhancedEnum.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq;
using EnhancedEnum.Tests.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnhancedEnum.Tests
{
    /// <summary>
    /// Defines test class FlagsTests.
    /// </summary>
    [TestClass]
    public class FlagsTests
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Init() { }

        /// <summary>
        /// Defines the test method Test_Errors.
        /// </summary>
        [TestMethod]
        public void Test_Errors()
        {
            var t = FlagsEnum.One;
            var t2 = t.Name;
            string t3 = null;
            t2.Should()
              .Be(t);
            string.IsNullOrEmpty(t3)
                  .Should()
                  .BeTrue();
            t3.Should()
              .BeNullOrEmpty();
            t = t3;
            t.Should()
             .Be(FlagsEnum.None);
        }

        /// <summary>
        /// Defines the test method Test_Flags.
        /// </summary>
        [TestMethod]
        public void Test_Flags()
        {
            var t = FlagsEnum.None;
            t.Should()
             .Be(FlagsEnum.None);

            t.Should()
             .Be(0);

            t = "One";
            t.Should()
             .Be(FlagsEnum.One);

            t = 0;
            t.Should()
             .Be(FlagsEnum.None);

            t++;
            t.Should()
             .Be(FlagsEnum.One);

            t = FlagsEnum.One | FlagsEnum.Two;
            t.Should()
             .Be(3);
            FlagsEnum.HasFlag(t, FlagsEnum.Two)
             .Should()
             .BeTrue();
            FlagsEnum.HasFlag(t, FlagsEnum.One)
                     .Should()
                     .BeTrue();
            FlagsEnum.HasFlag(t, FlagsEnum.Four)
                     .Should()
                     .BeFalse();
            t.HasFlag(FlagsEnum.Two)
             .Should()
             .BeTrue();
            t.HasFlag(FlagsEnum.Four)
             .Should()
             .BeFalse();
        }

        [TestMethod]
        public void Compare_Tests()
        {
            var flags = FlagsEnum.One | FlagsEnum.Two | FlagsEnum.Four | FlagsEnum.Eight;
            var flags2 = FlagsEnum.One | FlagsEnum.Two | FlagsEnum.Four | FlagsEnum.Eight;
            var flags3 = FlagsEnum.One | FlagsEnum.Two | FlagsEnum.Eight;
            FlagsEnum.Values.Where(x => x.Value > 0).ToList().ForEach(x => x.HasFlag(x).Should().BeTrue());
            flags.CompareTo(flags2)
                 .Should()
                 .Be(0);
            flags.CompareTo(flags3)
                 .Should()
                 .NotBe(0);
            var noFlag = FlagsEnum.None;
            noFlag.Should()
                  .Be(0);
            noFlag.HasFlag(0)
                  .Should()
                  .BeTrue();
        }
    }
}
