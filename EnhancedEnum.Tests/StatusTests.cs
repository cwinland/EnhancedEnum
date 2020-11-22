// ***********************************************************************
// Assembly         : EnhancedEnum.Tests
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-16-2020
// ***********************************************************************
// <copyright file="StatusTests.cs" company="EnhancedEnum.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Tests for EnhancedNum class.</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using EnhancedEnum.Tests.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnhancedEnum.Tests
{
    /// <summary>
    /// Defines test class EnhancedEnumTests.
    /// </summary>
    [TestClass]
    public class StatusTests
    {
        #region Fields

        private StatusEnum enhancedEnum;
        private FlagsEnum flagsEnum;

        private RegularEnum regularEnum;
        private string t;
        private StatusEnum t2;
        private int t3;
        private StatusEnum t4;
        private StatusEnum t5;
        private IEnumerable<StatusEnum> vals;

        #endregion

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            StatusEnum.ThrowOnError = false;
            t = StatusEnum.Running;
            t2 = StatusEnum.Stopped;
            t3 = StatusEnum.Error;
            vals = StatusEnum.Values;
            t4 = t3;
            t5 = t;
        }

        /// <summary>
        /// Defines the test method CompareEnums.
        /// </summary>
        [TestMethod]
        public void CompareEnums()
        {
            var sRegular = RegularEnum.Running.ToString();
            string sEnhanced = StatusEnum.Running;
            string sFlags = FlagsEnum.Four;

            var iRegular = (int)RegularEnum.Running;
            int iEnhanced = StatusEnum.Running;
            int iFlags = FlagsEnum.Four;

            // String to Enum
            regularEnum = (RegularEnum)Enum.Parse(typeof(RegularEnum), sRegular);
            regularEnum = Enum.Parse<RegularEnum>(sRegular);
            enhancedEnum = sEnhanced;
            flagsEnum = sFlags;

            // Int to Enum
            regularEnum = (RegularEnum)iRegular;
            enhancedEnum = iEnhanced;
            flagsEnum = iFlags;

            // Flag Manipulation
            regularEnum = RegularEnum.Stopped | RegularEnum.Error;
            flagsEnum = FlagsEnum.Four | FlagsEnum.Eight;
            regularEnum.HasFlag(RegularEnum.Stopped);
            flagsEnum.HasFlag(FlagsEnum.Four);

            // regularEnum.HasFlag(2); // Compile Error
            flagsEnum.HasFlag(2); // Valid check

            // Errors on conversion: Configurable to be NULL / throw exception.
            Func<RegularEnum> regularAct = () => regularEnum = (RegularEnum)55; // Assigns 55
            regularAct.Should()
                      .NotThrow();
            regularEnum.Should()
                       .Be(55);

            regularAct =
                () => regularEnum =
                    (RegularEnum)Enum.Parse(typeof(RegularEnum), "Running1"); // throws ArgumentException
            regularAct.Should()
                      .Throw<ArgumentException>();

            Func<StatusEnum> enhancedAct = () => enhancedEnum = 55; // returns null;
            enhancedAct.Should()
                       .NotThrow();
            enhancedEnum.Should()
                        .BeNull();

            enhancedAct = () => enhancedEnum = "Running1"; // returns null;
            enhancedAct.Should()
                       .NotThrow();
            enhancedEnum.Should()
                        .BeNull();

            StatusEnum.ThrowOnError = true;
            enhancedAct = () => enhancedEnum = 55; // throws InvalidOperationException
            enhancedAct.Should()
                       .Throw<InvalidOperationException>();
            enhancedAct = () => enhancedEnum = "Running1"; // throws InvalidOperationException
            enhancedAct.Should()
                       .Throw<InvalidOperationException>();
        }

        /// <summary>
        /// Defines the test method Hashcode_ShouldBeValue.
        /// </summary>
        [TestMethod]
        public void Hashcode_ShouldBeValue()
        {
            StatusEnum.Running.GetHashCode()
                      .Should()
                      .Be(5);

            StatusEnum.Stopped.GetHashCode()
                      .Should()
                      .Be(2);

            StatusEnum.Error.GetHashCode()
                      .Should()
                      .Be(3);
        }

        /// <summary>
        /// Defines assignment tests.
        /// </summary>
        [TestMethod]
        public void Assignments_ShouldBeSuccessful()
        {
            t.Should()
             .Be("In Process");

            t2.Should()
              .Be(StatusEnum.Stopped);

            t3.Should()
              .Be(StatusEnum.Error.Value);
            t4.Description.Should()
              .Be(StatusEnum.Error.ToString());
            t5.Description.Should()
              .Be("Indicates Running");
            t5.Name.Should()
              .NotBe(t5.Description);
            t4.Name.Should()
              .Be(t4.Description);
            t5.Name.Should()
              .Be(t5.ToString());
            t5.Name.Should()
              .Be("In Process");
            vals.Should()
                .HaveCount(StatusEnum.Count);
            t3.Should()
              .Be(t4);

            StatusEnum val = "Error1";

            val.Should()
               .BeNull();

            val = 12;
            val.Should()
               .BeNull();

            val = "In Process";
            val.Should()
               .Be(StatusEnum.Running);

            val = 5;
            val.Should()
               .Be(StatusEnum.Running);
        }

        /// <summary>
        /// Defines value equality tests.
        /// </summary>
        [TestMethod]
        public void Equality_ValuesShouldMatch()
        {
            (t3 == t4).Should()
                      .BeTrue();
            t3.Equals(t4)
              .Should()
              .BeTrue();
            t2.Equals(t4)
              .Should()
              .BeFalse();
            (t3 != t4).Should()
                      .BeFalse();
            (t2 == t4).Should()
                      .BeFalse();
            (t2 != t4).Should()
                      .BeTrue();
            (t3 >= t4).Should()
                      .BeTrue();
            (t2 <= t4).Should()
                      .BeTrue();
            (t2 > t4).Should()
                     .BeFalse();
            (t2 < t4).Should()
                     .BeTrue();
            t.Equals(t5)
             .Should()
             .BeTrue();

            // ReSharper disable once SuspiciousTypeConversion.Global
            t5.Equals(t)
              .Should()
              .BeTrue();
            (t5 == t).Should()
                     .BeTrue();
            t5.Should()
              .Be(t);
            t.Should()
             .Be(t5);

            t2 = StatusEnum.Error;
            t2.HasFlag(StatusEnum.Error)
              .Should()
              .BeFalse();
        }

        /// <summary>
        /// Defines naming equality tests.
        /// </summary>
        [TestMethod]
        public void Equality_NamesShouldMatch()
        {
            StatusEnum.Running.Should()
                      .BeGreaterThan(StatusEnum.Error);
            StatusEnum.Stopped.Should()
                      .BeLessThan(StatusEnum.Error);
            StatusEnum.Error.Should()
                      .BeLessThan(StatusEnum.Running);
            StatusEnum.Running.Should()
                      .BeGreaterOrEqualTo(StatusEnum.Error);
            StatusEnum.Stopped.Should()
                      .BeLessOrEqualTo(StatusEnum.Error);
            StatusEnum.Error.Should()
                      .BeLessOrEqualTo(StatusEnum.Running);
        }

        /// <summary>
        /// Defines the test method Name_ParseStrings.
        /// </summary>
        [TestMethod]
        public void Name_ParseStrings()
        {
            StatusEnum.Convert("In Process")
                      .Should()
                      .Be(StatusEnum.Running);
            StatusEnum.Convert("Error")
                      .Should()
                      .Be(StatusEnum.Error);
            StatusEnum.Convert("Stopped")
                      .Should()
                      .Be(StatusEnum.Stopped);
        }

        /// <summary>
        /// Defines the test method Name_ParseNumbers.
        /// </summary>
        [TestMethod]
        public void Name_ParseNumbers()
        {
            StatusEnum.Convert(5)
                      .Should()
                      .Be(StatusEnum.Running);
            StatusEnum.Convert(3)
                      .Should()
                      .Be(StatusEnum.Error);
            StatusEnum.Convert(2)
                      .Should()
                      .Be(StatusEnum.Stopped);
        }

        /// <summary>
        /// Defines the test method Loop_TestNames.
        /// </summary>
        [TestMethod]
        public void Loop_TestNames()
        {
            StatusEnum.Values.Should()
                      .Contain(StatusEnum.Running);
            StatusEnum.Values.Should()
                      .Contain(StatusEnum.Error);
            StatusEnum.Values.Should()
                      .Contain(StatusEnum.Stopped);
        }
    }
}
