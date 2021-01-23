namespace EnhancedEnum.Tests.Enums
{
    /// <summary>
    ///   Phyr Event Type.
    /// </summary>
    public sealed class TestEnum : EnhancedEnum<int, TestEnum>
    {
        #region Fields

        /// <summary>
        ///   Created Job.
        /// </summary>
        public static readonly TestEnum Created = new TestEnum();

        /// <summary>
        ///   Creating Job.
        /// </summary>
        public static readonly TestEnum Creating = new TestEnum();

        /// <summary>
        ///   Running Job.
        /// </summary>
        public static readonly TestEnum Performing = new TestEnum();

        /// <summary>
        ///   Job Completed.
        /// </summary>
        public static readonly TestEnum Performed = new TestEnum();

        /// <summary>
        ///   Job State Changing.
        /// </summary>
        public static readonly TestEnum StateChanged = new TestEnum();

        /// <summary>
        ///   Job Failed.
        /// </summary>
        public static readonly TestEnum FailedState = new TestEnum();

        /// <summary>
        ///   Job Queued and ready to run.
        /// </summary>
        public static readonly TestEnum EnqueuedState = new TestEnum();

        #endregion

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="TestEnum"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TestEnum(string value) => Convert(value);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="TestEnum"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TestEnum(int value) => Convert(value);
    }
}
