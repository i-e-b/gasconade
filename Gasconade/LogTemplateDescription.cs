namespace Gasconade
{
    /// <summary>
    /// Description data from a templated log message
    /// </summary>
    public class LogTemplateDescription
    {
        /// <summary>
        /// An overall description of the log message and its general meaning
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Optional: A description of why this log might be triggered
        /// </summary>
        public string Causes { get; set; }

        /// <summary>
        /// Optional: A description of action to be taken based on the log being triggered
        /// </summary>
        public string Actions { get; set; }

        /// <summary>
        /// If true, the message has been retired, and is recorded for historical purposes only
        /// </summary>
        public bool IsObsolete { get; set; }

        /// <summary>
        /// Optional: (if `IsObsolete` is true) A description of why the message was retired
        /// </summary>
        public string RetirementMessage { get; set; }
    }
}