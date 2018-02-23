using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using UnityEngine;

public class CommandList : MonoBehaviour
{
    [SerializeField] private RectTransform _commandButtonParent;
    [SerializeField] private CommandButton _commandButtonPrefab;
    private List<string> _wordList;
    private NetworkedPlayer np;

    private void Awake()
    {
        np = GetComponent<NetworkedPlayer>();
        np.NetworkStartEvent += NetworkStart;
        np.DoCommandEvent += DoCommand;
        np.NewCommandsEvent += NewCommands;
        np.NewWordListEvent += NewWordList;
    }

    private void NewCommandButton(int commandId)
    {
        GameObject obj = Instantiate(_commandButtonPrefab.gameObject, _commandButtonParent);
        var button = obj.GetComponent<CommandButton>();
        button.Init(np, commandId, "asd " + Random.value);
    }

    private void NetworkStart()
    {
        np.networkObject.Networker.IteratePlayers(SendCommandsToPlayer);
    }

    private void SendCommandsToPlayer(NetworkingPlayer player)
    {
        print(player.Ip);
        //np.networkObject.SendRpc(player, CommandsBehavior.RPC_NEW_COMMANDS, 0);
    }

    private void NewCommands(RpcArgs args)
    {
        var newCommandId = args.GetNext<int>();
        NewCommandButton(newCommandId);
        BMSLogger.Instance.Log("New command: " + newCommandId);
    }

    private void NewWordList(RpcArgs args)
    {
        var wordListBytes = args.GetNext<byte[]>();
        var wordList = wordListBytes.Deserialize<List<string>>();
        _wordList = wordList;

        foreach (string s in wordList)
        {
            BMSLogger.Instance.Log(s);
        }
    }

    private void DoCommand(RpcArgs args)
    {
    }
}