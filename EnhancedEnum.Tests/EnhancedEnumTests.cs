using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace EnhancedEnum.Tests
{
    [TestClass]
    public class EnhancedEnumTests
    {
        private string t;
        private StatusTest t2;
        private int t3;
        private IEnumerable<StatusTest> vals;
        private StatusTest t4;
        private StatusTest t5;

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

        [TestMethod]
        public void Enum_Assignments()
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
        }

        [TestMethod]
        public void Enum_Equality()
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

        [TestMethod]
        public void Name_Equality()
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
