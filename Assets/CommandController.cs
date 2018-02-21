using System;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Command
{
    public int CommandIndex;
    public NetworkingPlayer Owner;
}

public class CommandController : CommandsBehavior
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

    private readonly List<Command> _activeCommandButtons = new List<Command>();
    [SerializeField] private CommandList _commandList;
    [SerializeField] private GoalCommand _goalCommand;
    [SerializeField] private TextMeshProUGUI debugText;

    public event Action<RpcArgs> NewCommandEvent;
    public event Action<RpcArgs> CommandDoneEvent;
    public event Action<RpcArgs> NewGoalEvent;

    private void Start()
    {
        _commandList.CommandControllerInstance = this;
    }

    protected override void NetworkStart()
    {
        base.NetworkStart();

        if (!networkObject.IsServer)
        {
            return;
        }

        // SERVER
        CommandsStrings.Shuffle();
        foreach (string cmd in CommandsStrings)
        {
            print(cmd);
        }

        var currentIndex = 0;
        const int commandsPerPlayer = 2;
        foreach (NetworkingPlayer networkingPlayer in NetworkManager.Instance.Networker.Players)
        {
            print(networkingPlayer.InstanceGuid);

            SendNewGoalToPlayer(networkingPlayer);
            //SetRandomGoal();

            for (var i = 0; i < commandsPerPlayer; i++)
            {
                SendCommandButtonsToPlayer(networkingPlayer, currentIndex);
                currentIndex++;
            }
        }
    }

    private void SendNewGoalToPlayer(NetworkingPlayer player)
    {
        networkObject.SendRpc(player, RPC_NEW_GOAL, Random.Range(0, CommandsStrings.Count));
    }

    /// <summary>
    ///     Send list of commandIndexes to NetworkingPlayer
    /// </summary>
    /// <param name="player">Player to send commandIndexes to</param>
    /// <param name="commandIndexes">Set of commandIndexes to send</param>
    private void SendCommandButtonsToPlayer(NetworkingPlayer player, params int[] commandIndexes)
    {
        foreach (int commandIndex in commandIndexes)
        {
            networkObject.SendRpc(player, RPC_NEW_COMMAND, commandIndex);

            Command newCommand;
            newCommand.CommandIndex = commandIndex;
            newCommand.Owner = player;
            _activeCommandButtons.Add(newCommand);
        }
    }

    // RPC_NEW_COMMAND
    public override void NewCommand(RpcArgs args)
    {
        var commandIndex = args.GetNext<int>();
        _commandList.AddCommandButton(commandIndex);
    }

    // RPC_COMMAND_DONE
    public override void CommandDone(RpcArgs args)
    {
        var doneIndex = args.GetNext<int>();
        print("Command " + doneIndex + " done! Current goal: " + _goalCommand.CurrentGoalIndex);
        debugText.text = "Current goal: " + _goalCommand.CurrentGoalIndex + "\nLast received: " + doneIndex;
        if (_goalCommand.CurrentGoalIndex == doneIndex)
        {
            // Send new goal to ourselves
            NetworkingPlayer sender = networkObject.Owner;
            SendNewGoalToPlayer(sender);
        }
    }

    // RPC_NEW GOAL
    public override void NewGoal(RpcArgs args)
    {
        var commandIndex = args.GetNext<int>();
        SetRandomGoal();
    }

    private int GetRandomGoal(int oldGoalIndex = -1)
    {
        int value;
        do
        {
            value = Random.Range(0, CommandsStrings.Count);
        } while (value == oldGoalIndex);

        return value;
    }

    private void SetRandomGoal()
    {
        _goalCommand.CurrentGoalIndex = GetRandomGoal(_goalCommand.CurrentGoalIndex);
    }

    public void CommandClicked(int commandIndex)
    {
        NetworkingPlayer owner = _activeCommandButtons.Find(x => x.CommandIndex == commandIndex).Owner;

        // Send clicked command to everyone
        networkObject.SendRpc(RPC_COMMAND_DONE, Receivers.All, commandIndex);
    }
}