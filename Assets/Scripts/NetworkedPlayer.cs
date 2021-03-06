using System;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

public class NetworkedPlayer : CommandsBehavior
{
    public static readonly List<string> CommandsStrings = new List<string>
    {
        "Fiddle the diddle",
        "Read the bible",
        "Whip the horses",
        "Eat cabbage",
        "Hammer the nail",
        "Tighten the whippermancer",
        // "Rock the bells",
        // "Dimper the dingdongs"
    };

    public event Action<RpcArgs> DoCommandEvent;
    public event Action<RpcArgs> NewCommandsEvent;
    public event Action<RpcArgs> NewWordListEvent;
    public event Action<RpcArgs> CommandTimerDoneEvent;
    public event Action NetworkStartEvent;

    public Scenario ActiveScenario {get; private set;}


    protected override void NetworkStart()
    {
        base.NetworkStart();

        BMSLogger.Instance.Log("Started");

        if (networkObject.IsServer)
        {
            // Shuffle wordlist, then send it to all players
            CommandsStrings.ShuffleInPlace();
            byte[] bytes = CommandsStrings.SerializeToByteArray();
            networkObject.SendRpc(RPC_NEW_WORD_LIST, Receivers.All, bytes);

            // Send some words to each player to be presented as buttons
            var wordsPerPlayer = 3;
            var wordsSent = 0;
            networkObject.Networker.IteratePlayers(
                player =>
                {
                    var wordIndexes = new List<int>();
                    for (int i = wordsSent; i < wordsSent + wordsPerPlayer; i++)
                    {
                        wordIndexes.Add(i);
                    }
                    wordsSent += wordsPerPlayer;
                    SendCommandsToPlayer(player, wordIndexes);
                }
            );

            ActiveScenario = new Scenario();
            ActiveScenario.OnComplete += () => {
                BMSLogger.Instance.Log("SCENARIO COMPLETE!");
            };
            ActiveScenario.OnProgressChanged += (p) => {
                BMSLogger.Instance.Log("PROGRESS :" + p);
            };
        }

        if (NetworkStartEvent != null)
        {
            NetworkStartEvent();
        }
    }

    private void SendCommandsToPlayer(NetworkingPlayer player, List<int> wordIndexes)
    {
        print(player.Ip);
        byte[] wordIndexesBytes = wordIndexes.SerializeToByteArray();
        networkObject.SendRpc(player, RPC_NEW_COMMANDS, wordIndexesBytes);
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

    public override void CommandTimerDone(RpcArgs args)
    {
        // if (CommandTimerDoneEvent != null)
        // {
        //     CommandTimerDoneEvent(args);
        // }

        if(networkObject.IsServer){
            ActiveScenario.DecreaseProgress();
        }
    }
}