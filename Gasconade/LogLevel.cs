namespace Gasconade
{
    /// <summary>
    /// Available log levels
    /// </summary>
    public enum LogLevel {
        /// <summary>
        /// Messages that are used for integration and diagnostic purposes, and should not be recorded in production software
        /// </summary>
        Diagnostic, 

        /// <summary>
        /// Informative messages that don't necessarily relate to an error condition
        /// </summary>
        Info,

        /// <summary>
        /// Informative messages that warn of unexpected or out-of-normal conditions that may result in an error condition later
        /// </summary>
        Warning,

        /// <summary>
        /// Message relates to an error condition that prevents normal completion of an operation
        /// </summary>
        Error,

        /// <summary>
        /// Message relates to a condition that prevents the system from operating, or providing the intended service
        /// </summary>
        Critical
    }
}