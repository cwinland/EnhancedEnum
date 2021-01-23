using EnhancedEnum.Tests.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnhancedEnum.Tests
{
    [TestClass]
    public class TryConvertTests
    {
        [TestMethod]
        public void Status_TryConvert()
        {
            StatusEnum.TryConvert(2, out var v);
            v.Should().NotBeNull();
            v.Should().Be(StatusEnum.Stopped);

            StatusEnum.TryConvert(1, out v);
            v.Should().BeNull();
        }
    }
}
