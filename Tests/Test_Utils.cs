using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using VersatileFileComparerHost;

namespace Tests
{
    [TestFixture]
    class Test_Utils
    {
        [Test]
        public void Test_ReplaceFront()
        {
            Assert.That("abc_abc".ReplaceFront("abc", "aaa"), Is.EqualTo("aaa_abc"));

            Assert.That("abc_abc".ReplaceFront("abcd","nothing"), Is.EqualTo("abc_abc"));
        }
    }
}
