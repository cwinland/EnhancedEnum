using System.ComponentModel;

namespace EnhancedEnum.Tests
{
    public sealed class StatusTest : EnhancedEnum<int, StatusTest>
    {
        [Description("Indicates Running")]
        public static readonly StatusTest Running = new StatusTest(1);

        public static readonly StatusTest Stopped = new StatusTest(2);

        public static readonly StatusTest Error = new StatusTest(3);

        private StatusTest(int val) : base(val)
        {
        }

        // This allows us to assign back to this class.
        public static implicit operator StatusTest(string value) => Convert(value);

        public static implicit operator StatusTest(int value) => Convert(value);
    }
}
