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
        public void Controllers_implement_IController()
        {
            var controllers = ControllerHandlers().Select(h => h.ComponentModel.Implementation).ToSet();
            var typedControllers = _types.Where(t => t.Is<IController>()).ToSet();

            controllers.SymmetricExceptWith(typedControllers);

            Assert.IsEmpty(controllers.ToArray());
        }

        [Test]
        public void Controllers_live_in_controllers_namespace()
        {
            var controllers = ControllerHandlers().Select(h => h.ComponentModel.Implementation).ToSet();
            var typesInControllersNamespace = _types.Where(t => t.Namespace == _type.Namespace).ToSet();

            controllers.SymmetricExceptWith(typesInControllersNamespace);

            Assert.IsEmpty(controllers.ToArray());
        }

        [Test]
        public void Controllers_have_Controller_name_suffix()
        {
            var controllers = ControllerHandlers().Select(h => h.ComponentModel.Implementation).ToSet();
            var namedControllers = _types.Where(t => t.Name.EndsWith("Controller")).ToSet();

            controllers.SymmetricExceptWith(namedControllers);

            Assert.IsEmpty(controllers.ToArray());
        }

        private IHandler[] ControllerHandlers()
        {
            return _container.Kernel.GetAssignableHandlers(typeof(IController));
        }
    }
}