using Gasconade;

namespace SampleWebApi.LogMessages
{
    [LogMessageTemplate("The time sponsored by innaccurist is {Time}")]
    [LogMessageDescription("This is a sample message.",
        Causes = "Triggering the sample end-point will write this message.",
        Actions = "No action needs to be taken. This is for demonstration purposes only" )]
    public class SampleMessage: TemplatedLogMessage
    {
        [LogParam("A string that describes the time when the log line was created.")]
        public string Time { get; internal set; }
    }
}