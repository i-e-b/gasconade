using System;

namespace Gasconade
{
    /// <summary>
    /// This attribute gives the string template of a log message.
    /// Replacements can be added in the style of `{PropertyName}`
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class LogMessageTemplateAttribute : Attribute
    {
        /// <summary>
        /// Message template
        /// </summary>
        public string MessageTemplate { get; }

        /// <summary>
        /// Gives the string template of a log message.
        /// Replacements can be added in the style of `{PropertyName}`
        /// </summary>
        public LogMessageTemplateAttribute(string messageTemplate)
        {
            MessageTemplate = messageTemplate;
        }
    }
}