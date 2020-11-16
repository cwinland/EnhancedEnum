# EnhancedEnum
Enumeration that auto converts to string, integer, as well as provides easy looping through the options.

# Feature Overview
1. Simple Assignments
2. Auto Data conversion
3. Configurable underlying data types
4. Configurable error handling
   
## Comparison of a regular enum and EnhancedEnum - Example
```
    // Assignments
    RegularEnum regularEnum = RegularEnum.Running;
    StatusEnum enhancedEnum = StatusEnum.Running;
    FlagsEnum flagsEnum = FlagsEnum.Four;

    string sRegular = RegularEnum.Running.ToString();
    string sEnhanced = StatusEnum.Running;
    string sFlags = FlagsEnum.Four;

    int iRegular = (int)RegularEnum.Running;
    int iEnhanced = StatusEnum.Running;
    int iFlags = FlagsEnum.Four;

    // String to Enum
    regularEnum = (RegularEnum)Enum.Parse(typeof(RegularEnum), sRegular);
    regularEnum = Enum.Parse<RegularEnum>(sRegular);
    enhancedEnum = sEnhanced;
    flagsEnum = sFlags;

    // Int to Enum
    regularEnum = (RegularEnum) iRegular;
    enhancedEnum = iEnhanced;
    flagsEnum = iFlags;

    // Flag Manipulation
    regularEnum = RegularEnum.Stopped | RegularEnum.Error;
    flagsEnum = FlagsEnum.Four | FlagsEnum.Eight;
    regularEnum.HasFlag(RegularEnum.Stopped);
    flagsEnum.HasFlag(FlagsEnum.Four);
    // regularEnum.HasFlag(2); // Compile Error
    flagsEnum.HasFlag(2); // Valid check

    // Errors on conversion: Configurable to be NULL / throw exception.
    regularEnum = (RegularEnum) 55; // Assigns 55
    regularEnum = (RegularEnum)Enum.Parse(typeof(RegularEnum), "Running1"); // throws ArgumentException
    enhancedEnum = 55; // returns null;
    enhancedEnum = "Running1"; // returns null;
    StatusEnum.ThrowOnError = true;
    enhancedEnum = 55; // throws InvalidOperationException
    enhancedEnum = "Running1"; // throws InvalidOperationException
```

# Feature Examples

## Auto data type conversion
```
    string t = StatusEnum.Running;
    StatusEnum t2 = StatusEnum.Stopped;
    int t3 = StatusEnum.Error;
    IEnumerable<StatusEnum> vals = StatusEnum.Values;
    StatusEnum t4 = t3;
    StatusEnum t5 = t;
```
## Flags
```
    var t = FlagsEnum.One | FlagsEnum.Two;
    t.HasFlag(FlagsEnum.Two); // Returns True
    t.HasFlag(FlagsEnum.Four); // Returns False
```

## Non Integer value enums
```
    var nie = NonIntEnum.December;
    DateTime d1 = NonIntEnum.December;
    string s1 = NonIntEnum.December;
    NonIntEnum nie4 = "December 1st";
    NonIntEnum nie5 = DateTime.Parse("12/1/2020");
```

# Usage - Simple Enum
```
    public sealed class StatusTest : EnhancedEnum<int, StatusTest>
    {
        [Description("Indicates Running")] // Description Property.
        [DisplayName("In Process")]        // Displays this text when converted to a string.
        [Value(5)]                         // Underlying value.
        public static readonly StatusTest Running = new StatusTest();

        // Gets the default value of 2 because it is the second one in the list. Can use ValueAttribute to override.
        public static readonly StatusTest Stopped = new StatusTest();

        // Gets the default value of 3 because it is the third one in the list. Can use ValueAttribute to override.
        public static readonly StatusTest Error = new StatusTest();

        // Operators allow conversion to this class from other values.
        public static implicit operator StatusTest(string value) => Convert(value);
        public static implicit operator StatusTest(int value) => Convert(value);
    }
```

# Usage - Flags Enum
```
    [Flag]
    public sealed class FlagsEnum : EnhancedEnum<int, FlagsEnum>
    {
        [Value(0)]
        public static readonly FlagsEnum None = new FlagsEnum();

        [Value(1)]
        public static readonly FlagsEnum One = new FlagsEnum();

        [Value(2)]
        public static readonly FlagsEnum Two = new FlagsEnum();

        [Value(4)]
        public static readonly FlagsEnum Four = new FlagsEnum();

        [Value(8)]
        public static readonly FlagsEnum Eight = new FlagsEnum();

        // This allows us to assign back to this class.
        public static implicit operator FlagsEnum(string value) => Convert(value);
        public static implicit operator FlagsEnum(int value) => Convert(value);
    }
```

# Usage - Non Integer Value Enum
```
    public sealed class NonIntEnum : EnhancedEnum<DateTime, NonIntEnum>
    {
        [Description("Indicates Month of December")]
        [DisplayName("December 1st")]
        [Value("12/1/2020")]
        public static readonly NonIntEnum December = new NonIntEnum();

        [Value("11/1/2020")]
        public static readonly NonIntEnum November = new NonIntEnum();

        [Value("1/1/2020")]
        public static readonly NonIntEnum January = new NonIntEnum();

        // This allows us to assign back to this class.
        public static implicit operator NonIntEnum(string value) => Convert(value);
        public static implicit operator NonIntEnum(DateTime value) => Convert(value);

        // Custom value type converter
        protected override DateTime TypeConverter(object value) => DateTime.Parse(value.ToString());
    }
```

# License
MIT License

Copyright (c) 2020 cwinland

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.