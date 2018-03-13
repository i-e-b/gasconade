using System;

namespace Gasconade.Tests.SampleMessages
{
    [LogMessageTemplate("When an {Problem} comes along, you must ship it.")]
    [Obsolete("Problem shipping system removed 2018-01-01")]
    public class OldMessage: TemplatedLogMessage {
        public string Problem { get; set; }
    }
}