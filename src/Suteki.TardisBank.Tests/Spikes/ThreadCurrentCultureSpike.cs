using System;

namespace Suteki.TardisBank.Tests.Spikes
{
    public class ThreadCurrentCultureSpike
    {
        public void SettingTheCultureAlsoSetsTheCurrencySymbol()
        {
            var languages = new[]
            {
                "en-GB",
                "en-US",
                "fr",
                "de-CH"
            };

            foreach (var language in languages)
            {
                TestCurrencySymbol(language);
            }
        }

        static void TestCurrencySymbol(string language)
        {
            var culture = new System.Globalization.CultureInfo(language);
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;

            Console.Out.WriteLine("{0}", culture.NumberFormat.CurrencySymbol);
            Console.WriteLine("{0}: {1}", language, 4.50M.ToString("c"));
        }
    }
}