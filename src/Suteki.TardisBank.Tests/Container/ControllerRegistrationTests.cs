using System;
using System.Linq;
using System.Web.Mvc;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.Windsor;
using NUnit.Framework;
using Suteki.TardisBank.Controllers;
using Suteki.TardisBank.IoC;

namespace Suteki.TardisBank.Tests.Container
{
    [TestFixture]
    public class ControllerRegistrationTests
    {
        private IWindsorContainer _container;
        private readonly Type _type = typeof (HomeController);
        private Type[] _types;

        [SetUp]
        public void SetUp()
        {
            _types = _type.Assembly.GetExportedTypes();
            _container = new WindsorContainer().Install(new ControllersInstaller());
        }


        [Test]
        public void All_controller_types_are_registered()
        {
            var registeredControllers = ControllerHandlers().Select(h => h.ComponentModel.Implementation).ToSet();
            var typedControllers = _types.Where(t => t.Is<IController>()).ToSet();
            var typesInControllersNamespace = _types.Where(t => t.Namespace == _type.Namespace).ToSet();
            var namedControllers = _types.Where(t => t.Name.EndsWith("controller", StringComparison.OrdinalIgnoreCase)).ToSet();

            Assert.That(registeredControllers.SetEquals(typedControllers));
            Assert.That(registeredControllers.SetEquals(typesInControllersNamespace));
            Assert.That(registeredControllers.SetEquals(namedControllers));
        }

        private IHandler[] ControllerHandlers()
        {
            return _container.Kernel.GetAssignableHandlers(typeof(IController));
        }
    }
}