using System.IO;
using Gasconade;

namespace SampleWebApi
{
    public class FileWriterLog : ILogListener
    {
        private readonly string _path;
        private static readonly object FileLock = new object();

        public FileWriterLog(string path)
        {
            _path = path;
        }

        public void HandleMessage(LogLevel level, string message, object data)
        {
            lock(FileLock){
                File.AppendAllText(_path, "\r\n" + message);
            }
        }
    }
}