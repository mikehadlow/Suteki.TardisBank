using System;
using System.Linq;
using Raven.Client;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Services
{
    public interface ISchedulerService
    {
        void ExecuteUpdates(DateTime now);
    }

    public class SchedulerService : ISchedulerService
    {
        readonly IDocumentSession session;

        public SchedulerService(IDocumentSession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Gets all outstanding scheduled updates and performs the update.
        /// </summary>
        public void ExecuteUpdates(DateTime now)
        {
            var results = session.Query<Child>()
                .ToList() // just until Raven gets patched. This query is going to be horribly inefficient.
                .Where(child => child.Account.PaymentSchedules.Any(s => s.NextRun <= now));

            foreach (var child in results)
            {
                child.Account.TriggerScheduledPayments(now);
            }
        }
    }
}