using System;
using System.Collections.Generic;
using System.IO;


namespace Template.mod
{
    class FavouriteChannelsHelper
    {
        private static List<string> _channels = new List<string>();
        private static string _favouriteChannelsFilePath;
        private static bool _isDirty = true;

        FavouriteChannelsHelper(string ownFolder)
        {
            _favouriteChannelsFilePath = ownFolder + "FavouriteChannels.config";
        }

        public static List<string> Channels
        {
            get
            {
                if (_isDirty)
                    _channels.Clear();
                if (_channels != null && _channels.Count > 0 ) return _channels;
                if (!File.Exists(_favouriteChannelsFilePath))
                {
                    File.Create(_favouriteChannelsFilePath).Close();
                    return null;
                }

                if (IsEmpty(_favouriteChannelsFilePath))
                {
                    return null;
                }

                Logging.WriteLog("File is not empty, gathering favourite channels!", LogLevel.INFO);
                using (StreamReader reader = new StreamReader(_favouriteChannelsFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (_channels != null) _channels.Add(line.Trim().ToLower());
                    }
                    _isDirty = false;
                }

                return null;
            }
        }

        public static void JoinChannels(List<string> channels)
        {
            foreach (string channel in channels)
            {
                App.ArenaChat.RoomEnter(channel);
            }
        }

        public static bool IsEmpty(string filePath)
        {
            return (new FileInfo(filePath).Length == 0);
        }

        public static List<string> AddChannel(string channel)
        {
            using (StreamWriter sw = new StreamWriter(_favouriteChannelsFilePath))
            {
                sw.WriteLine(channel);    
            }
            _isDirty = true;
            return Channels;
        }

        public static void RemoveChannel(string channel)
        {
            List<string> channels = Channels;

            //Sort the list for alphabeticals sake?
            //I'm not sure how to do this off the top of my head
            //channels = channels.Sort(new Comparison<string>())

            if (channels.Contains(channel))
            {
                Channels.Remove(channel);
            }
            List<string> fileChannels = new List<string>();

            using (StreamReader reader = new StreamReader(_favouriteChannelsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    fileChannels.Add(line.Trim().ToLower());
                }
            }
            if (fileChannels.Contains(channel))
            {
                fileChannels.Remove(channel);
            }

            //Flush the file to disk so its actually saved
            using (StreamWriter sw = new StreamWriter(_favouriteChannelsFilePath))
            {
                if (!IsEmpty(_favouriteChannelsFilePath))
                {
                    //Lets back up first shall we?
                    File.Copy(_favouriteChannelsFilePath, _favouriteChannelsFilePath + ".bak");
                    //Dangerous!
                    File.Delete(_favouriteChannelsFilePath);
                }
                foreach (string c in channels)
                {
                    sw.WriteLine(c);
                }
            }
        }
    }
}
