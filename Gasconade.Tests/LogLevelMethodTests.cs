using Gasconade.Tests.Helpers;
using Gasconade.Tests.SampleMessages;
using NUnit.Framework;

namespace Gasconade.Tests
{
    [TestFixture]
    public class LogLevelMethodTests {
        [Test]
        public void diagnostic_level() {
            var dummyListener = new DummyListener(); Log.AddListener(dummyListener);

            Log.Diagnostic(new TestMessage { Target = "Paul", Subject = "Phil" });

            Assert.That(dummyListener.messages, Contains.Item("Diagnostic: Phil said something about Paul's mum"));
        }
        [Test]
        public void info_level() {
            var dummyListener = new DummyListener(); Log.AddListener(dummyListener);

            Log.Info(new TestMessage { Target = "Paul", Subject = "Phil" });

            Assert.That(dummyListener.messages, Contains.Item("Info: Phil said something about Paul's mum"));
        }
        [Test]
        public void warning_level() {
            var dummyListener = new DummyListener(); Log.AddListener(dummyListener);

            Log.Warning(new TestMessage { Target = "Paul", Subject = "Phil" });

            Assert.That(dummyListener.messages, Contains.Item("Warning: Phil said something about Paul's mum"));
        }
        [Test]
        public void error_level() {
            var dummyListener = new DummyListener(); Log.AddListener(dummyListener);

            Log.Error(new TestMessage { Target = "Paul", Subject = "Phil" });

            Assert.That(dummyListener.messages, Contains.Item("Error: Phil said something about Paul's mum"));
        }
        [Test]
        public void critical_level() {
            var dummyListener = new DummyListener(); Log.AddListener(dummyListener);

            Log.Critical(new TestMessage { Target = "Paul", Subject = "Phil" });

            Assert.That(dummyListener.messages, Contains.Item("Critical: Phil said something about Paul's mum"));
        }
    }
}
