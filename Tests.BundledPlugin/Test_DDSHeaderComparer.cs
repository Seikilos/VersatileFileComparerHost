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
    class Test_DDSHeaderComparer
    {
        public static string CreateFullPathToDDS(string filename)
        {
            var assembly = typeof(Test_DDSHeaderComparer).Assembly;
            var uri = new Uri(assembly.CodeBase);
            var localDirectory = Path.GetDirectoryName(uri.LocalPath);

            var final = Path.Combine(localDirectory, "..", "..", "..", "TestData", "DDS", filename);
            Assert.That(File.Exists(final), $"Path {final} must exist");
            return final;
        }

        [Test]
        public void Test_accept_DDS()
        {
            Assert.That(new DDSHeaderComparer().WantsToHandle("foo.dds"), Is.True, "Accepts dds");
            Assert.That(new DDSHeaderComparer().WantsToHandle("foo.DDS"), Is.True, "Accepts all upper");
            Assert.That(new DDSHeaderComparer().WantsToHandle("foo.DdS"), Is.True, "Accepts mixed");
        }

        [Test]
        public void Test_DXT1_To_Itself_must_be_ok()
        {
            var fileA = CreateFullPathToDDS("sample_dxt1_no_mips.dds");
            var fileB = CreateFullPathToDDS("sample_dxt1_no_mips.dds");

            var dds = new DDSHeaderComparer();
            dds.Init(null);

            Assert.That(() => dds.Handle(fileA, fileB), Throws.Nothing, "Must not throw on same file");
        }

        [Test]
        public void Test_DXT1_To_Same_With_Mips()
        {
            var fileA = CreateFullPathToDDS("sample_dxt1_no_mips.dds");
            var fileB = CreateFullPathToDDS("sample_dxt1_mips.dds");
            var mockIo = Substitute.For< IIO >();
            mockIo.ReadFile(fileA).Returns(new FileStream(fileA,FileMode.Open, FileAccess.Read,FileShare.ReadWrite));
            mockIo.ReadFile(fileB).Returns(new FileStream(fileB,FileMode.Open, FileAccess.Read,FileShare.ReadWrite));

            var dds = new DDSHeaderComparer();
            dds.Init(mockIo);

            Assert.That(() => dds.Handle(fileA, fileB), Throws.Exception.With.Property(nameof(Exception.Message)).Contains("mip"), "The difference in the files is located in mips");
        }

    }
}
