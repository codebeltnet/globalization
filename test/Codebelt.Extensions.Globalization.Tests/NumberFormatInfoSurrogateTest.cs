using System.Globalization;
using System.Reflection;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Codebelt.Extensions.Globalization
{
    public class NumberFormatInfoSurrogateTest : Test
    {
        public NumberFormatInfoSurrogateTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldCopyAllPropertiesFromNumberFormatInfo_WhenNumberFormatInfoIsProvided()
        {
            var assembly = typeof(CultureInfoExtensions).Assembly;
            var type = assembly.GetType("Codebelt.Extensions.Globalization.NumberFormatInfoSurrogate");
            var ctor = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new[] { typeof(NumberFormatInfo) },
                null);

            var nfi = new CultureInfo("da-DK", false).NumberFormat;
            var sut = ctor.Invoke(new object[] { nfi });

            Assert.Equal(nfi.CurrencyDecimalDigits, (int)type.GetProperty("CurrencyDecimalDigits").GetValue(sut));
            Assert.Equal(nfi.CurrencyDecimalSeparator, (string)type.GetProperty("CurrencyDecimalSeparator").GetValue(sut));
            Assert.Equal(nfi.CurrencyGroupSeparator, (string)type.GetProperty("CurrencyGroupSeparator").GetValue(sut));
            Assert.Equal(nfi.CurrencyNegativePattern, (int)type.GetProperty("CurrencyNegativePattern").GetValue(sut));
            Assert.Equal(nfi.CurrencyPositivePattern, (int)type.GetProperty("CurrencyPositivePattern").GetValue(sut));
            Assert.Equal(nfi.CurrencySymbol, (string)type.GetProperty("CurrencySymbol").GetValue(sut));
            Assert.Equal(nfi.DigitSubstitution, (DigitShapes)type.GetProperty("DigitSubstitution").GetValue(sut));
            Assert.Equal(nfi.NaNSymbol, (string)type.GetProperty("NaNSymbol").GetValue(sut));
            Assert.Equal(nfi.NegativeInfinitySymbol, (string)type.GetProperty("NegativeInfinitySymbol").GetValue(sut));
            Assert.Equal(nfi.NegativeSign, (string)type.GetProperty("NegativeSign").GetValue(sut));
            Assert.Equal(nfi.NumberDecimalDigits, (int)type.GetProperty("NumberDecimalDigits").GetValue(sut));
            Assert.Equal(nfi.NumberDecimalSeparator, (string)type.GetProperty("NumberDecimalSeparator").GetValue(sut));
            Assert.Equal(nfi.NumberGroupSeparator, (string)type.GetProperty("NumberGroupSeparator").GetValue(sut));
            Assert.Equal(nfi.NumberNegativePattern, (int)type.GetProperty("NumberNegativePattern").GetValue(sut));
            Assert.Equal(nfi.PerMilleSymbol, (string)type.GetProperty("PerMilleSymbol").GetValue(sut));
            Assert.Equal(nfi.PercentDecimalDigits, (int)type.GetProperty("PercentDecimalDigits").GetValue(sut));
            Assert.Equal(nfi.PercentDecimalSeparator, (string)type.GetProperty("PercentDecimalSeparator").GetValue(sut));
            Assert.Equal(nfi.PercentGroupSeparator, (string)type.GetProperty("PercentGroupSeparator").GetValue(sut));
            Assert.Equal(nfi.PercentNegativePattern, (int)type.GetProperty("PercentNegativePattern").GetValue(sut));
            Assert.Equal(nfi.PercentPositivePattern, (int)type.GetProperty("PercentPositivePattern").GetValue(sut));
            Assert.Equal(nfi.PercentSymbol, (string)type.GetProperty("PercentSymbol").GetValue(sut));
            Assert.Equal(nfi.PositiveInfinitySymbol, (string)type.GetProperty("PositiveInfinitySymbol").GetValue(sut));
            Assert.Equal(nfi.PositiveSign, (string)type.GetProperty("PositiveSign").GetValue(sut));

            TestOutput.WriteLine($"CurrencySymbol: {(string)type.GetProperty("CurrencySymbol").GetValue(sut)}");
        }
    }
}
