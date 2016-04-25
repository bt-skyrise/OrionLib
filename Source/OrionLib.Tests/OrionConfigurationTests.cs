using System;
using System.Security.Policy;
using NUnit.Framework;

namespace OrionLib.Tests
{
    public class OrionConfigurationTests
    {
        [Test]
        public void Can_Create_Orion_Configuration()
        {
            var config = new OrionConfiguration(new Url("http://orion-address.com"), "service", "/subService");

            Assert.That(config.OrionUrl, Is.EqualTo(new Url("http://orion-address.com")));
            Assert.That(config.Service, Is.EqualTo("service"));
            Assert.That(config.ServicePath, Is.EqualTo("/subService"));
        }

        [Test]
        public void Default_Service_And_Service_Path_Are_Empty()
        {
            var config = new OrionConfiguration(new Url("http://orion-address.com"));

            Assert.That(config.Service, Is.EqualTo(string.Empty));
            Assert.That(config.ServicePath, Is.EqualTo("/"));
        }

        [Test]
        public void Service_Path_Must_Start_With_Slash()
        {
            Assert.That(() => new OrionConfiguration(new Url("http://orion-address.com"), "service", "subService"),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}