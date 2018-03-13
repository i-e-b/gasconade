using System;
using Gasconade;

namespace ExternalLogDefinitions
{
    [LogMessageTemplate("I'm a poor {Subsystem}. My legs are old and bent. My ears are grizzled. My nose is knackered.")]
    [LogMessageDescription("Denotes that the system is facing some kind of hardship",
        Causes = "Happens when processes ids get too low")]
    [Obsolete("This message has been retired and will no longer be sent")]
    public class DecrepitMessage : TemplatedLogMessage
    {
        [LogParam("The subsystem experiencing problems")]
        public string Subsystem { get; set; }
    }
}