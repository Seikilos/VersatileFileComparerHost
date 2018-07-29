using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BundledPlugin;
using BundledPlugin.DDS;
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

        private static IIO CreateMock(string fileA, string fileB)
        {
            var mockIo = Substitute.For<IIO>();

            // use callback to prevent the mock to cache the object, this would return the already used and not reset stream
            mockIo.ReadFile(fileA)
                .Returns(info => new FileStream(fileA, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            mockIo.ReadFile(fileB)
                .Returns(info => new FileStream(fileB, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

            return mockIo;
        }


        [Test]
        public void Test_DXT1_To_Itself_must_be_ok()
        {
            var fileA = CreateFullPathToDDS("sample_dxt1_no_mips.dds");
            var fileB = CreateFullPathToDDS("sample_dxt1_no_mips.dds");
            var mockIo = CreateMock(fileA, fileB);

            var dds = new DDSHeaderComparer();
            dds.Init(mockIo);

            Assert.That(() => dds.Handle(fileA, fileB), Throws.Nothing, "Must not throw on same file");
        }

        [Test]
        public void Test_DXT1_To_Same_With_Mips()
        {
            var fileA = CreateFullPathToDDS("sample_dxt1_no_mips.dds");
            var fileB = CreateFullPathToDDS("sample_dxt1_mips.dds");

            var mockIo = CreateMock(fileA, fileB);


            var dds = new DDSHeaderComparer();
            dds.Init(mockIo);

            var ex = Assert.Throws<Exception>(() => dds.Handle(fileA, fileB));
            Assert.That(ex.Message.ToLower(), Does.Contain("mip"),
                "The difference in the files is located in mips");
        }


        [Test]
        public void Test_DXT1_To_DXT5()
        {
            var fileA = CreateFullPathToDDS("sample_dxt1_no_mips.dds");
            var fileB = CreateFullPathToDDS("sample_dxt5_no_mips.dds");

            var mockIo = CreateMock(fileA, fileB);

            var dds = new DDSHeaderComparer();
            dds.Init(mockIo);

            Assert.That(() => dds.Handle(fileA, fileB),
                Throws.Exception.With.Property(nameof(Exception.Message)).Contains("DXT1").And.Property(nameof(Exception.Message)).Contains("DXT5"));
        }


        [Test]
        public void Test_L8_Mips_No_Mips()
        {
            var fileA = CreateFullPathToDDS("sample_L8_mips.dds");
            var fileB = CreateFullPathToDDS("sample_L8_no_mips.dds");

            var mockIo = CreateMock(fileA, fileB);

            var dds = new DDSHeaderComparer();
            dds.Init(mockIo);

            var ex = Assert.Throws<Exception>(() => dds.Handle(fileA, fileB));


            Assert.That(ex.Message.ToLower(), Does.Contain("mip"),
                "The difference in the files is located in mips");
        }

        [Test]
        public void Test_DXT1_To_L8()
        {
            var fileA = CreateFullPathToDDS("sample_DXT1_no_mips.dds");
            var fileB = CreateFullPathToDDS("sample_L8_no_mips.dds");

            var mockIo = CreateMock(fileA, fileB);

            var dds = new DDSHeaderComparer();
            dds.Init(mockIo);

            var ex = Assert.Throws<Exception>(() => dds.Handle(fileA, fileB));

            Assert.That(ex.Message.ToLower(), Does.Contain("luminance"));
        }


        /// <summary>
        /// This test should ouptut both, different format and different mip level
        /// </summary>
        [Test]
        public void Test_DXT1_Mips_To_DXT5_No_Mips()
        {
            var fileA = CreateFullPathToDDS("sample_dxt1_mips.dds");
            var fileB = CreateFullPathToDDS("sample_dxt5_no_mips.dds");

            var mockIo = CreateMock(fileA, fileB);

            var dds = new DDSHeaderComparer();
            dds.Init(mockIo);

            var ex = Assert.Throws<Exception>(() => dds.Handle(fileA, fileB));

            Assert.That(ex.Message.ToLower(), Does.Contain("dxt1").And.Contains("dxt5").And.Contains("mips"));
        }

        [Test]
        public void Test_L8_To_L16_And_Mips()
        {
            Assert.Fail("Unclear why 16f header has a broken FOURCC of 'o' ");
            var fileA = CreateFullPathToDDS("sample_L8_no_mips.dds");
            var fileB = CreateFullPathToDDS("sample_16f_mips_special_settings.dds");

            var mockIo = CreateMock(fileA, fileB);

            var dds = new DDSHeaderComparer();
            dds.Init(mockIo);

            var ex = Assert.Throws<Exception>(() => dds.Handle(fileA, fileB));

            Assert.That(ex.Message.ToLower(), Does.Contain("dxt1").And.Contains("dxt5").And.Contains("mips"));
        }
    }
}
