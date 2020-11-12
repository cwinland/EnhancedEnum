using EnhancedEnum;
using System.ComponentModel;

namespace EnhancedEnum.Tests
{
    public sealed class StatusTest : EnhancedEnum<string, int, StatusTest>
    {
        [Description("Indicates Running")]
        public static readonly StatusTest Running = new StatusTest(nameof(Running), 1);

        public static readonly StatusTest Stopped = new StatusTest(nameof(Stopped), 2);
        public static readonly StatusTest Error = new StatusTest(nameof(Error), 3);

        private StatusTest(string stringValue, int val) : base(stringValue, val)
        {
        }

        // This allows us to assign back to this class.
        public static implicit operator StatusTest(string value) => Convert(value);

        public static implicit operator StatusTest(int value) => Convert(value);
    }
}
