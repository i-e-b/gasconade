using System;

namespace Gasconade
{
    /// <summary>
    /// This attribute gives a description of a log template replacement property in the log message class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class LogParamAttribute : Attribute
    {
        /// <summary>
        /// Description of the log parameter
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gives a description of a log template replacement parameter
        /// </summary>
        public LogParamAttribute(string description)
        {
            Description = description;
        }
    }
}