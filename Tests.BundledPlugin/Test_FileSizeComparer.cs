using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundledPlugin;
using Contracts;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Tests.BundledPlugin
{
    [TestFixture]
    public class Test_FileSizeComparer
    {
        public static Stream MakeStreamFromString( string text )
        {
            var stream = new MemoryStream();
            var sw = new StreamWriter(stream);


            sw.Write( text );
            sw.Flush();
            stream.Position = 0;

            return stream;
        }


        [Test]
        public void Test_Comparison_Fails_For_Different_Files()
        {
            // This test just checks the file size comparer, not the matching algorithm
            var mockIo = Substitute.For< IIO >();

            var comparer = new FileSizeComparer();
            comparer.Init(mockIo);

            var fileA = "a.file";
            var fileB = "different.foo";

            Assert.That(comparer.WantsToHandle(fileA), Is.True);
            Assert.That(comparer.WantsToHandle(fileB), Is.True);

            // Setup the io to return different sizes
            mockIo.ReadFile(fileA).Returns(MakeStreamFromString("a"));
            mockIo.ReadFile(fileB).Returns(MakeStreamFromString("different"));

            Assert.That( () => comparer.Handle(fileA, fileB), Throws.Exception);
        }

        [Test]
        public void Test_Comparison_Is_Valid_For_Same_File_Sizes()
        {
            // This test expects the files to be of equal length and must not fail
            var mockIo = Substitute.For< IIO >();


            var comparer = new FileSizeComparer();
            comparer.Init(mockIo);

            var fileA = "a.file";
            var fileB = "different.foo";

            Assert.That(comparer.WantsToHandle(fileA), Is.True);
            Assert.That(comparer.WantsToHandle(fileB), Is.True);

            // Mock the io to check for file size only
            mockIo.ReadFile(fileA).Returns(MakeStreamFromString("123"));
            mockIo.ReadFile(fileB).Returns(MakeStreamFromString("abc"));

            Assert.That(() => comparer.Handle(fileA, fileB), Throws.Nothing);
        }
    }
}
