using Gasconade.Tests.Helpers;
using Gasconade.Tests.SampleMessages;
using NUnit.Framework;

namespace Gasconade.Tests
{
    [TestFixture]
    public class BasicTests
    {
        [Test]
        public void log_messages_can_be_converted_to_strings()
        {
            var subject = new TestMessage { Target = "Paul", Subject = "Phil" };
            var result = subject.ToString();

            Assert.That(result, Is.EqualTo("Phil said something about Paul's mum"));
        }
        
        [Test]
        public void log_messages_can_have_escaped_braces()
        {
            var subject = new EscapedMessage { Unescaped = "Changed", Escaped = "NotChanged" };
            var result = subject.ToString();

            Assert.That(result, Is.EqualTo("This has been Changed and this has not {Escaped} been changed"));
        }

        [Test]
        public void incomplete_messages_get_a_standard_template (){
            var subject = new IncompleteMessage{ DemoParam = "Data" };
            var result = subject.ToString();

            Assert.That(result, Is.EqualTo("Message of type: IncompleteMessage; DemoParam = 'Data'"));
        }

        [Test]
        public void template_messages_that_dont_match_class_properties_get_a_placeholder (){
            var subject = new MismatchMessage{ Target = "The target property" };
            var result = subject.ToString();

            Assert.That(result, Is.EqualTo("The target property is ok, but {?Subject} is not somthing in this message"));
        }

        [Test]
        public void null_properties_are_replaced_with_special_placeholders (){
            var subject = new MismatchMessage{ Target = null };
            var result = subject.ToString();

            Assert.That(result, Is.EqualTo("<null> is ok, but {?Subject} is not somthing in this message"));
        }

        [Test]
        public void properties_that_throw_exceptions_are_replaced_with_a_special_placeholder ()
        {
            var subject = new FaultyMessage();
            var result = subject.ToString();

            Assert.That(result, Is.EqualTo("When an <error> comes along, you must whip it."));
        }

        [Test]
        public void can_extract_descriptions_from_templated_log_message()
        {
            var subject = new TestMessage { Target = "Paul", Subject = "Phil" };
            var result = subject.GetDescription();

            Assert.That(result.Actions, Is.EqualTo("The target should be encouraged to say something about the subject's face. Monitor to see that stress levels return to normal"));
            Assert.That(result.Causes, Is.EqualTo("Happens when stress levels are higher than normal"));
            Assert.That(result.Description, Is.EqualTo("Denotes that one employee is trying to insult another"));
        }

        [Test]
        public void static_logger_writes_expanded_messages_to_consumers() {
            var dummyListener = new DummyListener();
            Log.AddListener(dummyListener);
            Log.Warning(new TestMessage { Target = "Paul", Subject = "Phil" });

            Assert.That(dummyListener.messages, Contains.Item("Warning: Phil said something about Paul's mum"));
        }
        
        [Test]
        public void adding_a_listener_more_than_once_results_in_repeated_messages() {
            var dummyListener = new DummyListener();
            Log.AddListener(dummyListener);
            Log.AddListener(dummyListener);
            Log.Warning(new TestMessage { Target = "Paul", Subject = "Phil" });

            Assert.That(dummyListener.messages.Count, Is.EqualTo(2));
        }

        [Test]
        public void faulty_listeners_are_skipped_and_subsequent_listeners_called () {
            var dummyListener = new DummyListener();
            var faultyListener = new FaultyListener();
            Log.AddListener(dummyListener);
            Log.AddListener(faultyListener);
            Log.AddListener(dummyListener);
            Log.Warning(new TestMessage { Target = "Paul", Subject = "Phil" });

            Assert.That(dummyListener.messages.Count, Is.EqualTo(2));
        }
    }
}
