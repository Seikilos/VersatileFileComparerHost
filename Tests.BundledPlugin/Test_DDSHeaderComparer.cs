using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundledPlugin;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Tests.BundledPlugin
{
    [TestFixture]
    class Test_DDSHeaderComparer
    {
        [Test]
        public void Test_accept_DDS()
        {
            Assert.That(new DDSHeaderComparer().WantsToHandle("foo.dds"), Is.True, "Accepts dds");
            Assert.That(new DDSHeaderComparer().WantsToHandle("foo.DDS"), Is.True, "Accepts all upper");
            Assert.That(new DDSHeaderComparer().WantsToHandle("foo.DdS"), Is.True, "Accepts mixed");
        }
    }
}
