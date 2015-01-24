using System.Collections.Generic;
using ScrollsModLoader.Interfaces;

namespace Template.mod
{
    class ChatHelper
    {
        private static InvocationInfo _info;
        public ChatHelper(InvocationInfo info)
        {
            _info = info;
        }

        public static void HandleChatMessage()
        {
            RoomChatMessageMessage message = _info.arguments[0] as RoomChatMessageMessage;
            if (message == null) return;
            RoomChatMessageMessage rcmm = message;
            if (rcmm.text.ToLower().Equals("/acc") || rcmm.text.ToLower().StartsWith("/addcurrentchannel"))
            {
                AddCurrentChannel();
            }
            else if (rcmm.text.ToLower().StartsWith("/ac") || rcmm.text.ToLower().StartsWith("/addchannel"))
            {
                AddChannel(rcmm.text);
            }
            else if (rcmm.text.ToLower().Equals("/rcc") || rcmm.text.ToLower().StartsWith("/removecurrentchannel"))
            {
                RemoveCurrentChannel();
            }
            else if (rcmm.text.ToLower().StartsWith("/rc") || rcmm.text.ToLower().StartsWith("/removechannel"))
            {
                RemoveChannel(rcmm.text);
            }
            else if (rcmm.text.ToLower().Equals("/lf") || rcmm.text.ToLower().Equals("/lsf") || rcmm.text.ToLower().Equals("/listfavourites"))
            {
                ListFavourites();
            }
            else if (rcmm.text.ToLower().Equals("/help favourites") || rcmm.text.ToLower().Equals("/fhelp") || rcmm.text.ToLower().Equals("/favouriteshelp"))
            {
                PrintCommands();
            }

        }

        private static void PrintCommands()
        {
            SendPrivateChatMessage("/acc, /addcurrentchannel -> Adds current channel to favourites");
            SendPrivateChatMessage("/rcc, /removecurrentchannel -> Removes current channel from favourites");
            SendPrivateChatMessage("/ac [channelname], /addchannel [channelname] -> Adds [channelname] to favourites");
            SendPrivateChatMessage("/rc [channelname], /removechannel [channelname] -> Removes [channelname] from favourites");
            SendPrivateChatMessage("/lsf, /listfavorites -> Lists your favourite channels");
            SendPrivateChatMessage("/fhelp, /fcommands, /help favourites -> Prints this command list");
        }

        private static void ListFavourites()
        {
            List<string> favouriteChannels = FavouriteChannelsHelper.Channels;
            foreach (string channel in favouriteChannels)
            {
                SendPrivateChatMessage(channel);
            }
        }

        private static void RemoveChannel(string channel)
        {
            string[] splitStrings = channel.Split(' ');
            if (splitStrings.Length < 2) return;
            for (int i = 1; i < splitStrings.Length; ++i)
            {
                if (!FavouriteChannelsHelper.Channels.Contains(splitStrings[i]))
                {
                    SendPrivateChatMessage("The Channel " + splitStrings[i] + "is not in your list of favourite channels!");
                }
                else
                {
                    SendPrivateChatMessage("Removing  " + splitStrings[i] + " from your list of favourite Channels!");
                    FavouriteChannelsHelper.RemoveChannel(splitStrings[i]);
                }
            }
        }

        private static void RemoveCurrentChannel()
        {
            string channelToRemove = App.ArenaChat.ChatRooms.GetCurrentRoomName().ToLower();
            if (!FavouriteChannelsHelper.Channels.Contains(channelToRemove))
            {
                SendPrivateChatMessage("The Channel " + channelToRemove + "is not in your list of favourite channels!");
            }
            else
            {
                SendPrivateChatMessage("Removing  " + channelToRemove + " from your list of favourite Channels!");
                FavouriteChannelsHelper.RemoveChannel(channelToRemove);
            }

        }

        private static void AddChannel(string channel)
        {
            string[] splitStrings = channel.Split(' ');
            if (splitStrings.Length < 2) return;
            for (int i = 1; i < splitStrings.Length; ++i)
            {
                if (FavouriteChannelsHelper.Channels.Contains(splitStrings[i]))
                {
                    SendPrivateChatMessage("The Channel " + splitStrings[i] + "is already in your list of favourite channels!");
                }
                else
                {
                    SendPrivateChatMessage("Adding " + splitStrings[i] + " To your list of favourite Channels!");
                    FavouriteChannelsHelper.AddChannel(splitStrings[i]);
                }
            }
        }

        private static void AddCurrentChannel()
        {
            string channelToAdd = App.ArenaChat.ChatRooms.GetCurrentRoomName().ToLower();
            if (FavouriteChannelsHelper.Channels.Contains(channelToAdd))
            {
                SendPrivateChatMessage("The Channel " + channelToAdd + "is already in your list of favourite channels!");
            }
            else
            {
                SendPrivateChatMessage("Adding " + channelToAdd + " To your list of favourite Channels!");
                FavouriteChannelsHelper.AddChannel(channelToAdd);
            }
        }

        public static void SendPrivateChatMessage(string message)
        {
            RoomChatMessageMessage rcmm = new RoomChatMessageMessage
            {
                @from = "<color=#ffffff>FavouriteChannels</color>",
                text = "<color=#ffffff>" + message + "</color>",
                roomName = App.ArenaChat.ChatRooms.GetCurrentRoomName()
            };
            App.ChatUI.handleMessage(rcmm);
            App.ArenaChat.ChatRooms.ChatMessage(rcmm);
        }
    }
}
