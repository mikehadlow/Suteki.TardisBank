// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Suteki.TardisBank.Events;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Tests.Model
{
    [TestFixture]
    public class WithdrawlCashTests
    {

        Parent parent;
        Child child;
        Parent somebodyElsesParent;

        [SetUp]
        public void SetUp()
        {
            parent = new Parent("Dad", "mike@mike.com", "xxx");
            child = parent.CreateChild("Leo", "leohadlow", "yyy");
            parent.MakePaymentTo(child, 10.00M);

            somebodyElsesParent = new Parent("Not Dad", "jon@jon.com", "zzz");

            DomainEvent.TurnOff();
        }

        [TearDown]
        public void TearDown()
        {
            DomainEvent.Reset();
        }


        [Test]
        public void Child_should_be_able_to_withdraw_cash()
        {
            child.WithdrawCashFromParent(parent, 2.30M, "For Toys");

            child.Account.Balance.ShouldEqual(7.70M);
            child.Account.Transactions[1].Amount.ShouldEqual(-2.30M);
            child.Account.Transactions[1].Description.ShouldEqual("For Toys");

            parent.Messages.Count.ShouldEqual(1);
            parent.Messages[0].Text.ShouldEqual("Leo would like to withdraw 2.30");
        }

        [Test, ExpectedException(typeof(CashWithdrawException), ExpectedMessage = "Not Your Parent")]
        public void Child_shold_not_be_able_to_withdraw_from_some_other_parent()
        {
            child.WithdrawCashFromParent(somebodyElsesParent, 2.30M, "for toys");
        }

        [Test, ExpectedException(typeof(CashWithdrawException),
            ExpectedMessage = "You can not withdraw 12.11 because you only have 10.00 in your account")]
        public void Child_should_not_be_able_to_withdraw_more_than_their_balance()
        {
            child.WithdrawCashFromParent(parent, 12.11M, "For Toys");
        }

        [Test]
        public void Should_raise_a_SendMessageEvent()
        {
            SendMessageEvent sendMessageEvent = null;

            DomainEvent.TestWith(@event => { sendMessageEvent = (SendMessageEvent)@event; });

            child.WithdrawCashFromParent(parent, 2.30M, "For Toys");

            sendMessageEvent.ShouldNotBeNull();
            sendMessageEvent.User.ShouldBeTheSameAs(parent);
            sendMessageEvent.Message.ShouldEqual("Leo would like to withdraw 2.30");
        }
    }
}
// ReSharper restore InconsistentNaming