using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Events
{
    public class NewParentCreatedEvent : IDomainEvent
    {
        public NewParentCreatedEvent(Parent parent)
        {
            Parent = parent;
        }

        public Parent Parent { get; private set; }
    }
}