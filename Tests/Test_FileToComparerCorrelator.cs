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
    public class Test_FileToComparerCorrelator
    {

        [Test]
        public void Test_No_Error_On_Match()
        {
            // Arrange
            var log = Substitute.For< Contracts.ILogger >();

            var correlator = new FileToComparerCorrelator(log);
            var mockComparer = Substitute.For< IVFComparer >();
            mockComparer.WantsToHandle(Arg.Any< string >()).Returns(true); // Handle all

            var compareEntry = new CompareEntry("foo","a.txt", "b.txt");

            // Act
            var result = correlator.MatchFilesToComparers(new List< IVFComparer > {mockComparer}, new List< CompareEntry >(){compareEntry}).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(1), "One compare entry should yield one match");
        }

        [Test]
        public void Test_Handle_Entries_Which_Have_No_Matches()
        {
            // Arrange
            var log = Substitute.For<Contracts.ILogger>();

            var correlator = new FileToComparerCorrelator(log);
            var mockComparer = Substitute.For<IVFComparer>();
            mockComparer.WantsToHandle(Arg.Any<string>()).Returns(true); // Handle all

            var compareEntry = new CompareEntry("foo", "a.txt", null);
            var compareEntry2 = new CompareEntry("foo2", null, "b.txt");

            // Act
            var result = correlator.MatchFilesToComparers(new List<IVFComparer> { mockComparer }, new List<CompareEntry>() { compareEntry, compareEntry2 }).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(0), "No comparer should be created if FileA or FileB are null");
            log.Received().Error(Arg.Any<string>(), Arg.Is<object[]>(o =>  o.OfType<string>().Any(a => a.Contains("foo"))));
            log.Received().Error(Arg.Any<string>(), Arg.Is<object[]>(o =>  o.OfType<string>().Any(a => a.Contains("foo2"))));
        }

        /// <summary>
        /// Test should check for an error when one does fully match and the other mock comparer only partially, which should not be possible
        /// </summary>
        [Test]
        public void Test_One_Match_Other_Not()
        {
            var log = Substitute.For< Contracts.ILogger >();

            var correlator = new FileToComparerCorrelator(log);
            var mockComparer = Substitute.For< IVFComparer >();
            mockComparer.WantsToHandle("a.txt").Returns(true); // handle only one file
            var mockComparer2 = Substitute.For< IVFComparer >();
            mockComparer2.WantsToHandle(Arg.Any<string>()).Returns(true);
            var compareEntry = new CompareEntry("foo","a.txt", "b.txt");


            // Act
            var result = correlator.MatchFilesToComparers(new List< IVFComparer > {mockComparer, mockComparer2}, new List< CompareEntry >(){compareEntry}).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(1), "The valid one should be added to list");
            Assert.That(result[0].Item2.First, Is.EqualTo(mockComparer2));
            log.Received().Error(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void Test_No_Match_Found()
        {
            var log = Substitute.For< Contracts.ILogger >();

            var correlator = new FileToComparerCorrelator(log);
            var mockComparer = Substitute.For< IVFComparer >(); // defaults to false for WantsToHandle
            var mockComparer2 = Substitute.For< IVFComparer >(); // defaults to false for WantsToHandle
            
            var compareEntry = new CompareEntry("foo","a.txt", "b.txt");

            // Act
            var result = correlator.MatchFilesToComparers(new List< IVFComparer > {mockComparer,mockComparer2}, new List< CompareEntry >(){compareEntry}).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(0), "No match in this situation but error");
            log.Received().Error(Arg.Any<string>(), Arg.Any<object[]>());
        }
    }
}
