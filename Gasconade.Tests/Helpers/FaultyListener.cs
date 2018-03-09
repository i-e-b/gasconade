using System;

namespace Gasconade.Tests.Helpers
{
    public class FaultyListener:ILogListener
    {
        public void HandleMessage(LogLevel level, string message, object data)
        {
            throw new Exception();
        }
    }
}