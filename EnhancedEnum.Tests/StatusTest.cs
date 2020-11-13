namespace EnhancedEnum.Tests
{
    public sealed class StatusTest : EnhancedEnum<int, StatusTest>
    {
        [Description("Indicates Running")]
        [DisplayName("In Process")]
        [Value(5)]
        // DisplayName is 'In Process' (because of the DisplayName attribute). Value is 5 (because of the Value attribute).
        public static readonly StatusTest Running = new StatusTest();

        // DisplayName is Stopped. Value is 2 (because it is the second one in the list).
        public static readonly StatusTest Stopped = new StatusTest();

        // DisplayName is Error. Value is 3 (because it is the third one in the list).
        public static readonly StatusTest Error = new StatusTest();

        // This allows us to assign back to this class.
        public static implicit operator StatusTest(string value) => Convert(value);

        public static implicit operator StatusTest(int value) => Convert(value);
    }
}
