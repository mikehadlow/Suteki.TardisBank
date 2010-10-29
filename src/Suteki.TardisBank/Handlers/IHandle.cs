using Suteki.TardisBank.Events;

namespace Suteki.TardisBank.Handlers
{
    public interface IHandle<in TEvent> where TEvent : class, IDomainEvent
    {
        void Handle(TEvent @event);
    }
}