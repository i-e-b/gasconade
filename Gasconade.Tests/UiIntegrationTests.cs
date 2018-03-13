using Gasconade.Tests.SampleMessages;
using NUnit.Framework;

namespace Gasconade.Tests
{
    [TestFixture]
    public class UiIntegrationTests {

        [Test]
        public void can_get_a_list_of_property_names_and_descriptions_for_a_type (){
            var subject = TemplatedLogMessage.GetPropertyDescriptions(typeof(TestMessage));

            Assert.That(subject, Has.Count.EqualTo(3));
            Assert.That(subject["Target"], Is.EqualTo("The target (receiver) of an insult"));
            Assert.That(subject["Subject"], Is.EqualTo("The sender of an insult (the active party)"));
            Assert.That(subject["Undescribed"], Is.EqualTo(""));
        }

        [Test]
        public void can_get_the_template_raw_text_from_a_type (){
            var subject = TemplatedLogMessage.GetTemplateText(typeof(TestMessage));

            Assert.That(subject, Is.EqualTo("{Subject} said something about {Target}'s mum"));
        }

        [Test]
        public void can_get_the_description_text_from_a_type (){
            var subject = TemplatedLogMessage.GetDescription(typeof(TestMessage));
            
            Assert.That(subject.Actions, Is.EqualTo("The target should be encouraged to say something about the subject's face. Monitor to see that stress levels return to normal"));
            Assert.That(subject.Causes, Is.EqualTo("Happens when stress levels are higher than normal"));
            Assert.That(subject.Description, Is.EqualTo("Denotes that one employee is trying to insult another"));
        }

    }
}