using System;

namespace EnhancedEnum.Tests.Enums
{
    /// <summary>
    /// Enum RegularEnum
    /// </summary>
    [Flags]
    public enum RegularEnum
    {
        /// <summary>
        /// The running
        /// </summary>
        Running = 1,
        /// <summary>
        /// The stopped
        /// </summary>
        Stopped = 2,
        /// <summary>
        /// The error
        /// </summary>
        Error = 4,
    }
}
