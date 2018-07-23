using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using NSubstitute;
using NUnit.Framework;
using VersatileFileComparerHost;

namespace Tests
{
    public class DummyComparer : IVFComparer
    {
        public void Init( IIO io )
        {
        }

        public bool WantsToHandle( string file )
        {
            return true;
        }
    }


    [TestFixture]
    public class Test_PluginLoader
    {
        public static Stream ReadThisAssemblyAsStream()
        {

            var thisAssembly = Assembly.GetExecutingAssembly().Location;

            var fs = new FileStream(thisAssembly, FileMode.Open, FileAccess.Read,FileShare.ReadWrite);
            return fs;
        }


        [Test]
        public void Test_AssemblyLoading_From_Disk()
        {
            var mockIo = Substitute.For< IIO >();

            mockIo.GetFiles("foo", Arg.Any<string>(), Arg.Any< bool >()).Returns(new List< string >
            {
                "foo/A.dll",
                "foo/B.dll"
            } );

            mockIo.ReadFile("foo/A.dll").Returns(ReadThisAssemblyAsStream());
            mockIo.ReadFile("foo/B.dll").Returns(ReadThisAssemblyAsStream());

            var pl = new PluginLoader(mockIo);

            pl.LoadFromPluginDirectory("foo");


            mockIo.Received().ReadFile("foo/A.dll");
            mockIo.Received().ReadFile("foo/B.dll");

        }


        [Test]
        public void Test_Discovering_Comparers_Is_Implemented()
        {
            var mockIo = Substitute.For< IIO >();

            var pl = new PluginLoader(mockIo);
            pl.AddAssembly(Assembly.GetExecutingAssembly());

            var comparers = pl.DiscoverAndInitializePlugins();

            Assert.That(comparers, Has.Count.EqualTo(1));

            Assert.That(comparers.First().GetType(), Is.EqualTo(typeof(DummyComparer)));

        }
    }
}
