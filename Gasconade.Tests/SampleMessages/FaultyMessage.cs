using System;

namespace Gasconade.Tests.SampleMessages
{
    [LogMessageTemplate("When an {Problem} comes along, you must whip it.")]
    public class FaultyMessage: TemplatedLogMessage {
        public string Problem { get{ throw new Exception("This message is lost");} }
    }
}