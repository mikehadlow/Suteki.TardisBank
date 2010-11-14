// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Suteki.TardisBank.Helpers;

namespace Suteki.TardisBank.Tests.Helpers
{
    [TestFixture]
    public class DateFomatterTests 
    {
        [Test]
        public void Should_return_the_correct_date_format()
        {
            GetJQueryDateFormatFor("en-GB").ShouldEqual("dd/mm/yy");
            GetJQueryDateFormatFor("en-US").ShouldEqual("m/d/yy");
            GetJQueryDateFormatFor("fr").ShouldEqual("dd/mm/yy");
            GetJQueryDateFormatFor("de-CH").ShouldEqual("dd.mm.yy");
            GetJQueryDateFormatFor("de").ShouldEqual("dd.mm.yy");
        }

        static string GetJQueryDateFormatFor(string language)
        {
            var culture = new System.Globalization.CultureInfo(language);
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;

            return DateFormatter.CurrentJQuery;
        }
    }
}
// ReSharper restore InconsistentNaming