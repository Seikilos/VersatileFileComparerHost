﻿using System;
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
        public void Test_One_Match_Other_Not()
        {
            // TODO use two comparers here instead of one
            var log = Substitute.For< Contracts.ILogger >();

            var correlator = new FileToComparerCorrelator(log);
            var mockComparer = Substitute.For< IVFComparer >();
            mockComparer.WantsToHandle("a.txt").Returns(true); // handle only one file
            var compareEntry = new CompareEntry("foo","a.txt", "b.txt");


            // Act
            var result = correlator.MatchFilesToComparers(new List< IVFComparer > {mockComparer}, new List< CompareEntry >(){compareEntry}).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(0), "No match in this situation but error");
            log.Received().Error(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Test]
        public void Test_No_Match_Found()
        {
            var log = Substitute.For< Contracts.ILogger >();

            var correlator = new FileToComparerCorrelator(log);
            var mockComparer = Substitute.For< IVFComparer >(); // defaults to false for WantsToHandle
            
            var compareEntry = new CompareEntry("foo","a.txt", "b.txt");

            // Act
            var result = correlator.MatchFilesToComparers(new List< IVFComparer > {mockComparer}, new List< CompareEntry >(){compareEntry}).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(0), "No match in this situation but error");
            log.Received().Error(Arg.Any<string>(), Arg.Any<object[]>());
        }
    }
}
