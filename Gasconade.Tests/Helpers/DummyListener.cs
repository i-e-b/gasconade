using System.Collections.Generic;

namespace Gasconade.Tests.Helpers
{
    public class DummyListener: ILogListener
    {
        public List<string> messages = new List<string>();

        public void HandleMessage(LogLevel level, string message, object data)
        {
            messages.Add(level + ": " + message);
        }
    }
}