using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using My.TaskManagement.Enums;

namespace EnhancedEnum.Tests
{
    [TestClass]
    public class OtherTests
    {
        [TestMethod]
        public void Test()
        {
            TestEnum.Creating.Should()
                    .Be("Creating");
            TestEnum.Creating.Should()
                    .Be(2);
            TestEnum.Creating.Should()
                    .NotBe(TestEnum.Created);
            TestEnum.Creating.Should()
                    .NotBe("Created");
            TestEnum.Creating.Should()
                    .NotBe(1);
        }
    }
}
