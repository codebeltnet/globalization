using System;
using System.Globalization;
using System.Reflection;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Codebelt.Extensions.Globalization
{
    public class DateTimeFormatInfoSurrogateTest : Test
    {
        public DateTimeFormatInfoSurrogateTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldCopyAllPropertiesFromDateTimeFormatInfo_WhenDateTimeFormatInfoIsProvided()
        {
            var assembly = typeof(CultureInfoExtensions).Assembly;
            var type = assembly.GetType("Codebelt.Extensions.Globalization.DateTimeFormatInfoSurrogate");
            var ctor = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new[] { typeof(DateTimeFormatInfo) },
                null);

            var dtfi = new CultureInfo("da-DK", false).DateTimeFormat;
            var sut = ctor.Invoke(new object[] { dtfi });

            Assert.Equal(dtfi.AMDesignator, (string)type.GetProperty("AMDesignator").GetValue(sut));
            Assert.Equal(dtfi.CalendarWeekRule, (CalendarWeekRule)type.GetProperty("CalendarWeekRule").GetValue(sut));
            Assert.Equal(dtfi.DateSeparator, (string)type.GetProperty("DateSeparator").GetValue(sut));
            Assert.Equal(dtfi.FirstDayOfWeek, (DayOfWeek)type.GetProperty("FirstDayOfWeek").GetValue(sut));
            Assert.Equal(dtfi.FullDateTimePattern, (string)type.GetProperty("FullDateTimePattern").GetValue(sut));
            Assert.Equal(dtfi.LongDatePattern, (string)type.GetProperty("LongDatePattern").GetValue(sut));
            Assert.Equal(dtfi.LongTimePattern, (string)type.GetProperty("LongTimePattern").GetValue(sut));
            Assert.Equal(dtfi.MonthDayPattern, (string)type.GetProperty("MonthDayPattern").GetValue(sut));
            Assert.Equal(dtfi.PMDesignator, (string)type.GetProperty("PMDesignator").GetValue(sut));
            Assert.Equal(dtfi.ShortDatePattern, (string)type.GetProperty("ShortDatePattern").GetValue(sut));
            Assert.Equal(dtfi.ShortTimePattern, (string)type.GetProperty("ShortTimePattern").GetValue(sut));
            Assert.Equal(dtfi.TimeSeparator, (string)type.GetProperty("TimeSeparator").GetValue(sut));
            Assert.Equal(dtfi.YearMonthPattern, (string)type.GetProperty("YearMonthPattern").GetValue(sut));
            Assert.Equal(dtfi.ShortestDayNames, (string[])type.GetProperty("ShortestDayNames").GetValue(sut));
            Assert.Equal(dtfi.AbbreviatedDayNames, (string[])type.GetProperty("AbbreviatedDayNames").GetValue(sut));
            Assert.Equal(dtfi.AbbreviatedMonthNames, (string[])type.GetProperty("AbbreviatedMonthNames").GetValue(sut));
            Assert.Equal(dtfi.AbbreviatedMonthGenitiveNames, (string[])type.GetProperty("AbbreviatedMonthGenitiveNames").GetValue(sut));

            TestOutput.WriteLine($"ShortDatePattern: {(string)type.GetProperty("ShortDatePattern").GetValue(sut)}");
        }
    }
}
