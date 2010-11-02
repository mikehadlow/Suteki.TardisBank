using System;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Events
{
    public class NewParentCreatedEvent : IDomainEvent
    {
        public NewParentCreatedEvent(Parent parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            Parent = parent;
        }

        public Parent Parent { get; private set; }
    }
}