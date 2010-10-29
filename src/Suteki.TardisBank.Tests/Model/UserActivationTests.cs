// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Suteki.TardisBank.Events;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Tests.Model
{
    [TestFixture]
    public class UserActivationTests 
    {

        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void ParentShouldNotBeActiveWhenCreated()
        {
            User parent = new Parent("Dad", "mike@mike.com", "xxx");
            parent.IsActive.ShouldBeFalse();
        }

        [Test]
        public void Parent_should_raise_an_event_when_created()
        {
            NewParentCreatedEvent newParentCreatedEvent = null;
            using(DomainEvent.TestWith(e => { newParentCreatedEvent = (NewParentCreatedEvent)e; }))
            {
                var parent = new Parent("Dad", "mike@mike.com", "xxx").Initialise();

                newParentCreatedEvent.ShouldNotBeNull();
                newParentCreatedEvent.Parent.ShouldBeTheSameAs(parent);
            }
        }

        [Test]
        public void Child_should_be_active_when_created()
        {
            User child = new Parent("Dad", "Mike@mike.com", "xxx").CreateChild("Leo", "leoahdlow", "bbb");
            child.IsActive.ShouldBeTrue();
        }
    }
}
// ReSharper restore InconsistentNaming