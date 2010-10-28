// ReSharper disable InconsistentNaming
using System;
using NUnit.Framework;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Tests.Model
{
    [TestFixture]
    public class PaymentSchedulingTests 
    {
        readonly DateTime startDate = new DateTime(2010, 10, 28);
        const Interval interval = Interval.Week;
        const decimal amount = 2.00M;
        const string description = "Weekly Pocket Money";

        Child child;

        [SetUp]
        public void SetUp()
        {
            child = new Parent("Dad", "mike@mike.com", "xxx")
                .CreateChild("Leo", "leohadlow", "yyy");
        }

        [Test]
        public void Should_be_able_to_add_a_schedule_to_an_account()
        {
            child.Account.AddPaymentSchedule(startDate, interval, amount, description);

            child.Account.PaymentSchedules.Count.ShouldEqual(1);
            child.Account.PaymentSchedules[0].NextRun.ShouldEqual(startDate);
            child.Account.PaymentSchedules[0].Interval.ShouldEqual(interval);
            child.Account.PaymentSchedules[0].Amount.ShouldEqual(amount);
            child.Account.PaymentSchedules[0].Description.ShouldEqual(description);
        }

        [Test]
        public void Should_be_able_to_trigger_the_payment()
        {
            child.Account.AddPaymentSchedule(startDate, interval, amount, description);

            child.Account.TriggerScheduledPayments(startDate);

            child.Account.Transactions.Count.ShouldEqual(1);
            child.Account.Transactions[0].Amount.ShouldEqual(amount);
            child.Account.Transactions[0].Description.ShouldEqual(description);
            child.Account.Transactions[0].Date.ShouldEqual(DateTime.Now.Date);

            var oneWeek = new TimeSpan(7, 0, 0, 0);
            var expecteNextRun = startDate + oneWeek;
            child.Account.PaymentSchedules[0].NextRun.ShouldEqual(expecteNextRun);
        }

        [Test]
        public void Triggering_payment_before_next_run_causes_nothing_to_happen()
        {
            child.Account.AddPaymentSchedule(startDate, interval, amount, description);
            child.Account.TriggerScheduledPayments(startDate.AddMinutes(-1));

            child.Account.Transactions.Count.ShouldEqual(0);
            child.Account.PaymentSchedules[0].NextRun.ShouldEqual(startDate);
        }
    }
}
// ReSharper restore InconsistentNaming