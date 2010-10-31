// ReSharper disable InconsistentNaming
using System.Linq;
using Raven.Client.Indexes;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Indexes
{
    public class Child_ByPendingSchedule : AbstractIndexCreationTask<Child>
    {
        public Child_ByPendingSchedule()
        {
            Map = children => from child in children
                              from schedule in child.Account.PaymentSchedules
                              select new { schedule.NextRun };
        }
    }
}
// ReSharper restore InconsistentNaming
