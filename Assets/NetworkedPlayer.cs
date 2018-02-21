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
    public event Action<RpcArgs> NewCommandEvent;
    public event Action<RpcArgs> CommandDoneEvent;
    public event Action<RpcArgs> NewGoalEvent;
    public event Action NetworkStartEvent;

    protected override void NetworkStart()
    {
        base.NetworkStart();

        BMSLogger.Instance.Log("Started");

        if (NetworkStartEvent != null)
        {
            NetworkStartEvent();
        }
    }

    // RPC_NEW_COMMAND
    public override void NewCommand(RpcArgs args)
    {
        if (NewCommandEvent != null)
        {
            NewCommandEvent(args);
        }
    }

    // RPC_COMMAND_DONE
    public override void CommandDone(RpcArgs args)
    {
        if (CommandDoneEvent != null)
        {
            CommandDoneEvent(args);
        }
    }

    // RPC_NEW GOAL
    public override void NewGoal(RpcArgs args)
    {
        if (NewGoalEvent != null)
        {
            NewGoalEvent(args);
        }
    }
}