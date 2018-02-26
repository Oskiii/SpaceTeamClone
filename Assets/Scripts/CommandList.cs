using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using UnityEngine;

public class CommandList : MonoBehaviour
{
    [SerializeField] private RectTransform _commandButtonParent;
    [SerializeField] private CommandButton _commandButtonPrefab;
    private List<int> _commandList;
    [SerializeField] private GoalCommand _goalCommand;
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

    private void NewCommandButton(int cmdIndex)
    {
        GameObject obj = Instantiate(_commandButtonPrefab.gameObject, _commandButtonParent);
        var button = obj.GetComponent<CommandButton>();

        string word = _wordList[cmdIndex];
        button.Init(np, cmdIndex, word);
        BMSLogger.Instance.Log("New command: " + word);
    }

    private void NetworkStart()
    {
    }

    private void NewCommands(RpcArgs args)
    {
        var commandListBytes = args.GetNext<byte[]>();
        _commandList = commandListBytes.Deserialize<List<int>>();

        foreach (int cmdIndex in _commandList)
        {
            NewCommandButton(cmdIndex);
        }
    }

    private void NewWordList(RpcArgs args)
    {
        var wordListBytes = args.GetNext<byte[]>();
        _wordList = wordListBytes.Deserialize<List<string>>();

        foreach (string s in _wordList)
        {
            BMSLogger.Instance.Log(s);
        }

        PickNewGoal();
    }

    private void PickNewGoal()
    {
        int goalIndex = Random.Range(0, _wordList.Count);
        print("Picking out of " + _wordList.Count + ", picked: " + goalIndex);
        _goalCommand.SetGoal(goalIndex, _wordList[goalIndex]);
    }

    private void DoCommand(RpcArgs args)
    {
        var doneIndex = args.GetNext<int>();
        BMSLogger.Instance.Log("Do command: " + doneIndex);

        if (_goalCommand.GoalIndex == doneIndex)
        {
            print("goal: " + _goalCommand.GoalIndex + " done: " + doneIndex);
            BMSLogger.Instance.Log("YAY OUR GOAL WAS SOLVED");
            PickNewGoal();
        }
    }
}