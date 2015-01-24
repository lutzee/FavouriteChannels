using System;
using ScrollsModLoader.Interfaces;
using Mono.Cecil;

namespace Template.mod
{
	public class FavouriteChannels : BaseMod, ICommListener
	{
	    public static string ModFolder { get; private set; }

	    //initialize everything here, Game is loaded at this point
        public FavouriteChannels()
		{
		    ModFolder = OwnFolder();
            App.Communicator.addListener(this);
        }


		public static string GetName ()
		{
			return "TemplateMod";
		}

		public static int GetVersion ()
		{
			return 1;
		}

		//only return MethodDefinitions you obtained through the scrollsTypes object
		//safety first! surround with try/catch and return an empty array in case it fails
	    public static MethodDefinition[] GetHooks(TypeDefinitionCollection scrollsTypes, int version)
	    {
	        try
	        {
	            return new[] {
	               scrollsTypes["MainMenu"].Methods.GetMethod("Start")[0],
                   scrollsTypes["Communicator"].Methods.GetMethod("sendRequest", new[]{typeof(Message)}),
                   scrollsTypes["ArenaChat"].Methods.GetMethod("RoomEnter")[0],
	            };
	        }
	        catch (Exception e)
	        {
	            Console.WriteLine(e);
	        }
	        return new MethodDefinition[] {};
	    }


	    public override void BeforeInvoke (InvocationInfo info)
		{
		    switch(info.targetMethod)
		    {
                case "Start":
                    FavouriteChannelsHelper.JoinChannels(FavouriteChannelsHelper.Channels);
                    break;
                case "sendRequest":

		            break;
                default:
                    throw new Exception("Invalid method! how did you manage that?");
		    }
		}

	    public override void AfterInvoke (InvocationInfo info, ref object returnValue)
		{
		}

		//override only when needed
		/*

		public override void ReplaceMethod (InvocationInfo info, out object returnValue)
		{
			returnValue = null;
		}

		public override bool WantsToReplace (InvocationInfo info)
		{
			return false;
		}

		*/
	    public void handleMessage(Message msg)
	    {
            Console.WriteLine(msg);
	    }

	    public void onReconnect()
	    {
	        Message.createMessage("reconnect","Reconnected!");
	    }
	}
}

