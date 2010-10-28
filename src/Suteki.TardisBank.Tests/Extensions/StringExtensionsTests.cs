using Suteki.TardisBank.Extensions;
using NUnit.Framework;
using System;

namespace Suteki.TardisBank.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void Should_decamel_helloworld()
        {
            String testString = "HelloWorldIAmYourNemesis";

            testString.DeCamel().ShouldEqual("Hello World I Am Your Nemesis");
        }

        [Test]
        public void Should_pretty_helloworld()
        {
            String testString = "hello_worldIAmYourNemesis";

            testString.Pretty().ShouldEqual("hello world I Am Your Nemesis");
        }
        
    }
}
