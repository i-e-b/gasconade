namespace Gasconade.Tests.SampleMessages
{
    [LogMessageTemplate("This has been {Unescaped} and this has not {{Escaped}} been changed")]
    public class EscapedMessage : TemplatedLogMessage
    {
        public string Unescaped { get; set; }
        public string Escaped { get; set; }
    }
}