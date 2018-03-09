namespace Gasconade
{
    /// <summary>
    /// Interface for injecting into the log class
    /// </summary>
    public interface ILogListener
    {
        /// <summary>
        /// Handle a message sent via the logging system. It is up to the listener to determine which levels
        /// should be responded to, and what actions to take
        /// </summary>
        /// <param name="level">Log level as stated by the caller</param>
        /// <param name="message">message from the log, with templates completed from data</param>
        /// <param name="data">the original object sent by caller</param>
        void HandleMessage(LogLevel level, string message, object data);
    }
}