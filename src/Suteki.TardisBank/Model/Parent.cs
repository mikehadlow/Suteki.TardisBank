using System;
using System.Collections.Generic;
using System.Linq;
using Suteki.TardisBank.Events;

namespace Suteki.TardisBank.Model
{
    public class Parent : User
    {
        public IList<ChildProxy> Children { get; private set; }
        public string ActivationKey { get; private set; }

        public Parent(string name, string userName, string password) : base(name, userName, password)
        {
            Children = new List<ChildProxy>();
        }

        // should be called when parent is first created.
        public Parent Initialise()
        {
            ActivationKey = Guid.NewGuid().ToString();
            DomainEvent.Raise(new NewParentCreatedEvent(this));
            return this;
        }

        public Child CreateChild(string name, string userName, string password)
        {
            var child = new Child(name, userName, password, Id);
            var childProxy = new ChildProxy(child.Id, name);
            Children.Add(childProxy);
            return child;
        }

        public void MakePaymentTo(Child child, decimal amount)
        {
            MakePaymentTo(child, amount, string.Format("Payment from {0}", Name));
        }

        public void MakePaymentTo(Child child, decimal amount, string description)
        {
            if (!HasChild(child))
            {
                throw new TardisBankException("{0} is not a child of {1}", child.Name, Name);
            }
            child.ReceivePayment(amount, description);
        }

        public bool HasChild(Child child)
        {
            return Children.Any(x => x.ChildId == child.Id);
        }

        public override void Activate()
        {
            ActivationKey = "";
            base.Activate();
        }
    }

    public class ChildProxy
    {
        public ChildProxy(string childId, string name)
        {
            ChildId = childId;
            Name = name;
        }

        public string ChildId { get; private set; }
        public string Name { get; private set; }
    }
}