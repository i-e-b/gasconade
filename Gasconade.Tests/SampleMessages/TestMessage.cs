namespace Gasconade.Tests.SampleMessages
{
    [LogMessageTemplate("{Subject} said something about {Target}'s mum")]
    [LogMessageDescription("Denotes that one employee is trying to insult another",
        Causes = "Happens when stress levels are higher than normal",
        Actions = "The target should be encouraged to say something about the subject's face. " +
                  "Monitor to see that stress levels return to normal" )]
    public class TestMessage : TemplatedLogMessage
    {
        [LogParam("The target (receiver) of an insult")]
        public string Target { get; set; }
        [LogParam("The sender of an insult (the active party)")]
        public string Subject { get; set; }
    }
}