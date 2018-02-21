using System;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using TMPro;
using UnityEngine;

public struct Command
{
    public int CommandIndex;
    public NetworkingPlayer Owner;
}

public class NetworkedPlayer : CommandsBehavior
{
    private readonly List<Command> _activeCommandButtons = new List<Command>();
    [SerializeField] private CommandList _commandList;
    [SerializeField] private GoalCommand _goalCommand;
    [SerializeField] private TextMeshProUGUI debugText;

    public event Action<RpcArgs> NewCommandEvent;
    public event Action<RpcArgs> CommandDoneEvent;
    public event Action<RpcArgs> NewGoalEvent;
    public event Action NetworkStartEvent;

    private void Start()
    {
        _commandList.NetworkedPlayerInstance = this;
    }

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