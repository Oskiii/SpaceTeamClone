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
    public event Action<RpcArgs> DoCommandEvent;
    public event Action NetworkStartEvent;

    protected override void NetworkStart()
    {
        base.NetworkStart();

        BMSLogger.Instance.Log("Started");

        if(networkObject.IsServer){
            
        }

        if (NetworkStartEvent != null)
        {
            NetworkStartEvent();
        }
    }

    public override void DoCommand(RpcArgs args)
    {
        DoCommandEvent(args);
        print("Do command");
    }
}