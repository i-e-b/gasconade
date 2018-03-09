namespace Gasconade.Tests.SampleMessages
{
    [LogMessageTemplate("{Target} is ok, but {Subject} is not somthing in this message")]
    public class MismatchMessage: TemplatedLogMessage{
        public string Target { get; set; }
    }
}