using System;

namespace Gasconade
{
    /// <summary>
    /// This attribute gives a description of the log message: the meaning, when it would be written, and actions that can be taken in response
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class LogMessageDescriptionAttribute : Attribute
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
        /// Gives the string template of a log message.
        /// Replacements can be added in the style of `{PropertyName}`
        /// </summary>
        public LogMessageDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}