using System;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

public struct Command
{
    public int CommandIndex;
    public NetworkingPlayer Owner;
}

public class NetworkedPlayer : CommandsBehavior
{
    public static readonly List<string> CommandsStrings = new List<string>
    {
        "Fiddle the diddle",
        "Read the bible",
        "Whip the horses",
        "Eat cabbage"
        //"Hammer the nail",
        //"Tighten the whippermancer",
        //"Rock the bells"
    };

    public event Action<RpcArgs> DoCommandEvent;
    public event Action<RpcArgs> NewCommandsEvent;
    public event Action<RpcArgs> NewWordListEvent;
    public event Action NetworkStartEvent;


    protected override void NetworkStart()
    {
        base.NetworkStart();

        BMSLogger.Instance.Log("Started");

        if (networkObject.IsServer)
        {
            // Shuffle wordlist, then send it to all players
            CommandsStrings.Shuffle();
            byte[] bytes = CommandsStrings.SerializeToByteArray();
            networkObject.SendRpc(RPC_NEW_WORD_LIST, Receivers.All, bytes);
        }

        if (NetworkStartEvent != null)
        {
            NetworkStartEvent();
        }
    }

    public override void DoCommand(RpcArgs args)
    {
        if (DoCommandEvent != null)
        {
            DoCommandEvent(args);
        }
        print("Do command");
    }

    public override void NewCommands(RpcArgs args)
    {
        if (NewCommandsEvent != null)
        {
            NewCommandsEvent(args);
        }
    }

    public override void NewWordList(RpcArgs args)
    {
        if (NewWordListEvent != null)
        {
            NewWordListEvent(args);
        }
    }
}