using EnhancedEnum.Tests.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [TestMethod]
        [DataRow("Created", "Job that is based on method '' has been created with id ''.")]
        [DataRow("Creating", "Creating a job that is based on method ''.")]
        [DataRow("Performing", "Starting to perform job ''.")]
        [DataRow("Performed", "Job '' has been performed.")]
        [DataRow("StateChanged", "Job '' state was changed from '' to ''.")]
        [DataRow("FailedState", "Error occurred during job '': .")]
        [DataRow("EnqueuedState", "Job '' state was queued.")]
        public void PhyrEvent_Message(string type, string expectedMessage)
        {
            TestEnum eventType = type;
            eventType.Should().NotBeNull();
            eventType.Should().Be(type);
            eventType.Name.ToUpperInvariant().Should().Be(type.ToUpperInvariant());
        }
    }
}
