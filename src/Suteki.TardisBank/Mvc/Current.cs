using System.Threading;

namespace Suteki.TardisBank.Mvc
{
    public class Current
    {
        public static string CurrencySymbol
        {
            get { return Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol; }
        }
    }
}