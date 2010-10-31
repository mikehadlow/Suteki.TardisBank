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
            // query is defined in this class: Suteki.TardisBank.Indexes.Child_ByPendingSchedule
            var results = session.Advanced
                .LuceneQuery<Child>("Child/ByPendingSchedule")
                .WhereLessThanOrEqual("NextRun", now)
                .WaitForNonStaleResults().ToList();

            foreach (var child in results)
            {
                child.Account.TriggerScheduledPayments(now);
            }
        }
    }
}