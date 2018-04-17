using System;
using Gasconade;

namespace SampleCoreWeb.LogMessages
{
    [LogMessageTemplate("An unexpected internal error occurred. Message = '{ErrMessage}'")]
    [LogMessageDescription("This message is logged by the last-resort error handler.",
        Causes = "An error occurred that was not otherwise expected by the engineering team.",
        Actions = "Inspect the logging system for full data and stack trace. Open a ticket with details with the QA team for triage." )]
    public class InternalErrorMessage : TemplatedLogMessage
    {
        [LogParam("A basic description of the error as raised by an internal system. This might not be useful on its own.")]
        public string ErrMessage { get { return TriggeringException.Message; } }

        /// <summary>
        /// This is stored for the log sender, and is not *directly* in the template
        /// </summary>
        public Exception TriggeringException { get; set; }
    }
}