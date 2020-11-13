// ***********************************************************************
// Assembly         : EnhancedEnum.Tests
// Author           : chris
// Created          : 11-12-2020
//
// Last Modified By : chris
// Last Modified On : 11-13-2020
// ***********************************************************************
// <copyright file="EnhancedEnumTests.cs" company="EnhancedEnum.Tests">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Tests for EnhancedNum class.</summary>
// ***********************************************************************
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace EnhancedEnum.Tests
{
    /// <summary>
    /// Defines test class EnhancedEnumTests.
    /// </summary>
    [TestClass]
    public class EnhancedEnumTests
    {
        private string t;
        private StatusTest t2;
        private int t3;
        private IEnumerable<StatusTest> vals;
        private StatusTest t4;
        private StatusTest t5;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            t = StatusTest.Running;
            t2 = StatusTest.Stopped;
            t3 = StatusTest.Error;
            vals = StatusTest.Values;
            t4 = t3;
            t5 = t;
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
              .Be(StatusTest.Stopped);

            t3.Should()
              .Be(StatusTest.Error.Value);
            t4.Description.Should()
              .Be(StatusTest.Error.ToString());
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
                .HaveCount(StatusTest.Count);
            t3.Should()
              .Be(t4);

            StatusTest val = "Error1";

            val.Should()
               .BeNull();

            val = 12;
            val.Should()
               .BeNull();

            val = "In Process";
            val.Should()
               .Be(StatusTest.Running);

            val = 5;
            val.Should()
               .Be(StatusTest.Running);
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
        }

        /// <summary>
        /// Defines naming equality tests.
        /// </summary>
        [TestMethod]
        public void Equality_NamesShouldMatch()
        {
            StatusTest.Running.Should()
                      .BeGreaterThan(StatusTest.Error);
            StatusTest.Stopped.Should()
                      .BeLessThan(StatusTest.Error);
            StatusTest.Error.Should()
                      .BeLessThan(StatusTest.Running);
            StatusTest.Running.Should()
                      .BeGreaterOrEqualTo(StatusTest.Error);
            StatusTest.Stopped.Should()
                      .BeLessOrEqualTo(StatusTest.Error);
            StatusTest.Error.Should()
                      .BeLessOrEqualTo(StatusTest.Running);
        }

        /// <summary>
        /// Defines the test method Name_ParseStrings.
        /// </summary>
        [TestMethod]
        public void Name_ParseStrings()
        {
            StatusTest.Convert("In Process")
                      .Should()
                      .Be(StatusTest.Running);
            StatusTest.Convert("Error")
                      .Should()
                      .Be(StatusTest.Error);
            StatusTest.Convert("Stopped")
                      .Should()
                      .Be(StatusTest.Stopped);
        }

        /// <summary>
        /// Defines the test method Name_ParseNumbers.
        /// </summary>
        [TestMethod]
        public void Name_ParseNumbers()
        {
            StatusTest.Convert(5)
                      .Should()
                      .Be(StatusTest.Running);
            StatusTest.Convert(3)
                      .Should()
                      .Be(StatusTest.Error);
            StatusTest.Convert(2)
                      .Should()
                      .Be(StatusTest.Stopped);
        }

        /// <summary>
        /// Defines the test method Loop_TestNames.
        /// </summary>
        [TestMethod]
        public void Loop_TestNames()
        {
            StatusTest.Values.Should()
                      .Contain(StatusTest.Running);
            StatusTest.Values.Should()
                      .Contain(StatusTest.Error);
            StatusTest.Values.Should()
                      .Contain(StatusTest.Stopped);
        }
    }
}
