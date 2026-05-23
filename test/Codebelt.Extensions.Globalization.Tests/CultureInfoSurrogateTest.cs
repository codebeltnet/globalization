using System.Reflection;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Codebelt.Extensions.Globalization
{
    public class CultureInfoSurrogateTest : Test
    {
        public CultureInfoSurrogateTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldAssignDateTimeAndNumberFormats_WhenSurrogatesAreProvided()
        {
            var assembly = typeof(CultureInfoExtensions).Assembly;
            var cultureInfoSurrogateType = assembly.GetType("Codebelt.Extensions.Globalization.CultureInfoSurrogate");
            var dtfiSurrogateType = assembly.GetType("Codebelt.Extensions.Globalization.DateTimeFormatInfoSurrogate");
            var nfiSurrogateType = assembly.GetType("Codebelt.Extensions.Globalization.NumberFormatInfoSurrogate");

            var dtSurrogate = System.Activator.CreateInstance(dtfiSurrogateType, nonPublic: true);
            var nfSurrogate = System.Activator.CreateInstance(nfiSurrogateType, nonPublic: true);

            var ctor = cultureInfoSurrogateType.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new[] { dtfiSurrogateType, nfiSurrogateType },
                null);

            var sut = ctor.Invoke(new[] { dtSurrogate, nfSurrogate });

            var dateTimeFormatProp = cultureInfoSurrogateType.GetProperty("DateTimeFormat", BindingFlags.Instance | BindingFlags.NonPublic);
            var numberFormatProp = cultureInfoSurrogateType.GetProperty("NumberFormat", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.Same(dtSurrogate, dateTimeFormatProp.GetValue(sut));
            Assert.Same(nfSurrogate, numberFormatProp.GetValue(sut));

            TestOutput.WriteLine($"DateTimeFormat assigned: {dateTimeFormatProp.GetValue(sut) != null}");
            TestOutput.WriteLine($"NumberFormat assigned: {numberFormatProp.GetValue(sut) != null}");
        }
    }
}
