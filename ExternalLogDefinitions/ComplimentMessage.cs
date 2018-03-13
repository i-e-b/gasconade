using Gasconade;

namespace ExternalLogDefinitions
{
    [LogMessageTemplate("{Subject} thinks that {Target} is awesome")]
    [LogMessageDescription("Denotes that one employee has a great deal of respect for another",
        Causes = "Happens when a high-five is needed but has no been recently supplied",
        Actions = "The subject and target should high-five. " +
                  "Management team should ensure that no fist-bumping or chest-bumping takes place." )]
    public class ComplimentMessage : TemplatedLogMessage
    {
        [LogParam("The target of admiration")]
        public string Target { get; set; }
        [LogParam("The employee having feelings of admiration")]
        public string Subject { get; set; }
    }
}