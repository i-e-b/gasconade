namespace Gasconade
{
    /// <summary>
    /// Log sender static container. Use this to send your log messages.
    /// Inject log suppliers to do the final writing
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Send a warning-level message
        /// </summary>
        /// <param name="logMessage">The message object. The class should have a LogMessageTemplate attribute</param>
        public static void Warning<T>(T logMessage) where T : TemplatedLogMessage
        {
        }
    }
}
