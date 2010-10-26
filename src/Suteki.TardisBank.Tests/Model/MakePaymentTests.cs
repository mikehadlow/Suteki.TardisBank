// ReSharper disable InconsistentNaming
using System;
using NUnit.Framework;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Tests.Model
{
    [TestFixture]
    public class MakePaymentTests
    {
        Parent parent;
        Child child;

        Parent somebodyElse;
        Child somebodyElsesChild;

        [SetUp]
        public void SetUp()
        {
            parent = new Parent("Mike Hadlow", "mike@yahoo.com");
            child = parent.CreateChild("Leo", "leohadlow", "xxx");

            somebodyElse = new Parent("John Robinson", "john@gmail.com");
            somebodyElsesChild = somebodyElse.CreateChild("Jim", "jimrobinson", "yyy");
        }

        [Test]
        public void Should_be_able_to_make_a_payment()
        {
            parent.MakePaymentTo(child, 2.30M);

            child.Account.Transactions.Count.ShouldEqual(1);
            child.Account.Transactions[0].Amount.ShouldEqual(2.30M);
            child.Account.Transactions[0].Description.ShouldEqual("Payment from Mike Hadlow");
            child.Account.Transactions[0].Date.ShouldEqual(DateTime.Now.Date);
            child.Account.Balance.ShouldEqual(2.30M);
        }

        [Test, ExpectedException(typeof(TardisBankException), ExpectedMessage = "Jim is not a child of Mike Hadlow")]
        public void Should_not_be_able_to_make_a_payment_to_somebody_elses_child()
        {
            parent.MakePaymentTo(somebodyElsesChild, 4.50M);
        }
    }
}
// ReSharper restore InconsistentNaming