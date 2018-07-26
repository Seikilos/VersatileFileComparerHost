using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using VersatileFileComparerHost.Implementation;

namespace Tests
{
    [TestFixture]
    class Test_Loggers
    {
        [Test]
        public void Test_CommonOutputLogger_Callbacks_Properly_Called()
        {
            // Arrange
            var actionOutput = Substitute.For< Action< string > >();
            var actionError = Substitute.For< Action< string > >();

            var cl = new CommonOutputLogger(actionOutput, actionError);


            // Act
            cl.Info("{0}", "Info");
            cl.Warning("{0}", "Warning");

            cl.Error("{0}", "Error");
            cl.Exception("Exception", new Exception("Text"));


            // Assert
            actionOutput.Received().Invoke(Arg.Is<string>(t => t.Contains("Info")));
            actionOutput.Received().Invoke(Arg.Is<string>(t => t.Contains("Warning")));

            actionError.Received().Invoke(Arg.Is<string>(t => t.Contains("Error")));
            actionError.Received().Invoke(Arg.Is<string>(t => t.Contains("Exception")));
            actionError.Received().Invoke(Arg.Is<string>(t => t.Contains("Text")));



        }

        [Test]
        public void Test_StringStreamLogger_Delegates_All_Calls_Properly()
        {
            // Arrange
            var slogger = new StringBuilderLogger();

            // Directly reset the string builders to check whether they are correctly used
            var sOut = new StringBuilder();
            var sErr = new StringBuilder();
            slogger.StringBuilderOutput = sOut;
            slogger.StringBuilderError = sErr;

            // Act
            slogger.Info("{0}","Info");
            slogger.Warning("{0}","Warning");
            slogger.Error("{0}","Error");
            slogger.Exception("Exception", new Exception("Text", new Exception("inner")));

            // Assert
            Assert.That(slogger.StringBuilderOutput.ToString(), Contains.Substring("Info"));
            Assert.That(slogger.StringBuilderOutput.ToString(), Contains.Substring("Warning"));

            Assert.That(slogger.StringBuilderError.ToString(), Contains.Substring("Error"));
            Assert.That(slogger.StringBuilderError.ToString(), Contains.Substring("Exception").And.Contains("Text").And.Contains("inner"));


        }

        #region Multi-threaded Logger Tests

        /// <summary>
        /// Invoke multiple tasks writing to the logger and expect no issues
        /// </summary>
        [Test]
        public void Test_Multiple_Threads()
        {
            var mlogger = new MultiThreadedCommonLogger();

            var tasks = new List< Task >();

            for (var i = 0; i < 100; ++i)
            {
                var copy = i;
                tasks.Add(Task.Run(() => mlogger.Error(copy.ToString())));
                tasks.Add(Task.Run(() => mlogger.Info(copy.ToString())));
            }

            Task.WaitAll(tasks.ToArray());

            var sbOut = new StringBuilder();
            var sbErr = new StringBuilder();
            mlogger.DumpAllLogs(s => sbOut.AppendLine(s), s => sbErr.AppendLine(s));

            Assert.That(sbOut.ToString(), Contains.Substring("99"), "On completion 99 must be part of the output stream");
            Assert.That(sbErr.ToString(), Contains.Substring("99"),"On completion 99 must be part of the error stream");

            Assert.That(mlogger.HadErrors, Is.True);
        }


        #endregion
    }
}
