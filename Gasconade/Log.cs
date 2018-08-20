using System.Collections.Generic;

namespace Gasconade
{
    /// <summary>
    /// Log sender static container. Use this to send your log messages.
    /// Inject log suppliers to do the final writing
    /// </summary>
    public static class Log
    {
        private static readonly List<ILogListener> _listeners = new List<ILogListener>();
        private static readonly object Lock = new object();

        /// <summary>
        /// Add a listener to static log.
        /// Every listener added will have a chance to respond to every log message sent
        /// </summary>
        public static void AddListener(ILogListener listener) {
            lock (Lock)
            {
                _listeners.Add(listener);
            }
        }
        
        /// <summary>
        /// Send a diagnostic-level message.
        /// Diagnostic messages are used for integration and diagnostic purposes, and should not be recorded in production software
        /// </summary>
        /// <param name="logMessage">The message object. The class should have a LogMessageTemplate attribute</param>
        public static void Diagnostic<T>(T logMessage) where T : TemplatedLogMessage
        {
            Trigger(LogLevel.Diagnostic, logMessage.ToString(), logMessage);
        }

        /// <summary>
        /// Send a info-level message.
        /// Info messages are messages that don't necessarily relate to an error condition
        /// </summary>
        /// <param name="logMessage">The message object. The class should have a LogMessageTemplate attribute</param>
        public static void Info<T>(T logMessage) where T : TemplatedLogMessage
        {
            Trigger(LogLevel.Info, logMessage.ToString(), logMessage);
        }

        /// <summary>
        /// Send a warning-level message.
        /// Warning messages are used for unexpected or out-of-normal conditions that may result in an error condition later
        /// </summary>
        /// <param name="logMessage">The message object. The class should have a LogMessageTemplate attribute</param>
        public static void Warning<T>(T logMessage) where T : TemplatedLogMessage
        {
            Trigger(LogLevel.Warning, logMessage.ToString(), logMessage);
        }

        /// <summary>
        /// Send a Error-level message.
        /// Error messages relate to an error conditions that prevents normal completion of an operation
        /// </summary>
        /// <param name="logMessage">The message object. The class should have a LogMessageTemplate attribute</param>
        public static void Error<T>(T logMessage) where T : TemplatedLogMessage
        {
            Trigger(LogLevel.Error, logMessage.ToString(), logMessage);
        }

        /// <summary>
        /// Send a Critical-level message.
        /// Critical messages relate to conditions that prevent the system from operating, or providing the intended service
        /// </summary>
        /// <param name="logMessage">The message object. The class should have a LogMessageTemplate attribute</param>
        public static void Critical<T>(T logMessage) where T : TemplatedLogMessage
        {
            Trigger(LogLevel.Critical, logMessage.ToString(), logMessage);
        }

        private static void Trigger(LogLevel logLevel, string message, TemplatedLogMessage data)
        {
            lock (Lock)
            {
                foreach (var listener in _listeners)
                {
                    try
                    {
                        listener.HandleMessage(logLevel, message, data);
                    }
                    catch
                    {
                        Ignore();
                    }
                }
            }
        }

        private static void Ignore() {}

        /// <summary>
        /// Remove all listeners from the static logger.
        /// All log messages will be ignored until further listeners are added
        /// </summary>
        public static void RemoveAllListeners()
        {
            lock (Lock)
            {
                _listeners.Clear();
            }
        }
    }
}

