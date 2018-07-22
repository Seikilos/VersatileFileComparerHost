using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using NSubstitute;
using NUnit.Framework;
using VersatileFileComparerHost;

namespace Tests
{
    [TestFixture]
    public class Test_FileGatherer
    {
        [Test]
        public void Test_Ctor_Checks_For_Directory_Existence()
        {
            var dirA = "a";
            var dirB = "b";

            var mockIo = Substitute.For<IIO>();
            mockIo.DirectoryExist(dirA).Returns(true);
            mockIo.DirectoryExist(dirB).Returns(true);

            new FileGatherer(mockIo, dirA, dirB);

            mockIo.Received().DirectoryExist(dirA);
            mockIo.Received().DirectoryExist(dirB);

        }

        [Test]
        public void Test_Creation_Of_List_Handles_Differences_Correctly()
        {
            var mockIo = Substitute.For<IIO>();
            mockIo.DirectoryExist(Arg.Any<string>()).Returns(true); // Irrelevant at this point

            mockIo.GetFiles("a", "*", true).Returns(new List<string>
            {
                @"a\a_only.txt",
                @"a\sub\common.txt",
            });
            mockIo.GetFiles("b", "*", true).Returns(new List<string>
            {
                    @"b\b_only.txt",
                    @"b\sub\common.txt",
            });

            var list = new FileGatherer(mockIo, "a", "b").CreateFilelist().ToList();

            Assert.That(list, Has.Count.EqualTo(3), "Expect three entries found");

            Assert.That(list.FirstOrDefault(l => l.CommonName.Contains("a_only")), Has.Property(nameof(CompareEntry.PathB)).Null, "a_only must exist only on the item 1 side");
            Assert.That(list.FirstOrDefault(l => l.CommonName.Contains("b_only")), Has.Property(nameof(CompareEntry.PathA)).Null, "b_only must exist only on the item 2 side");

            Assert.That(list.FirstOrDefault(l => l.PathA != null && l.PathB != null), Has.Property(nameof(CompareEntry.CommonName)).Contains("common"), "b_only must exist only on the item 2 side");


        }
    }
}
