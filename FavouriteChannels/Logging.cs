using System.IO;

namespace Template.mod
{
    class Logging
    {
        private const string LogKey = "[TestMod] ";
        private static string _filePath;
        Logging(string filePath)
        {
            _filePath = filePath + Path.DirectorySeparatorChar + "FavouriteChannels.log";
        }

        public static void WriteLog(string message, LogLevel logLevel)
        {
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
            }

            StreamWriter sw = new StreamWriter(_filePath);
            switch (logLevel)
            {
                case LogLevel.CRITICAL:
                    sw.WriteLine(LogKey + " " + "[!!CRITICAL!!]" + " " + message);
                    break;
                case LogLevel.ERROR:
                    sw.WriteLine(LogKey + " " + "[!ERROR!]" + " " + message);
                    break;
                case LogLevel.WARNING:
                    sw.WriteLine(LogKey + " " + "[WARNING]" + " " + message);
                    break;
                case LogLevel.VERBOSE:
                    sw.WriteLine(LogKey + " " + "[INFO VERBOSE]" + " " + message);
                    break;
                case LogLevel.INFO:
                    sw.WriteLine(LogKey + " " + "[INFO]" + " " + message);
                    break;
            }
        }
    }
}
