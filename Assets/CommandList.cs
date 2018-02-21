using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandList : MonoBehaviour
{
    private readonly List<ButtonToCommand> _activeButtons = new List<ButtonToCommand>();
    [SerializeField] private CommandButton _commandButtonPrefab;
    [SerializeField] private LayoutGroup _commandParent;
    public CommandController CommandControllerInstance;

    public void AddCommandButton(int commandIndex)
    {
        GameObject commandObj = Instantiate(_commandButtonPrefab.gameObject, _commandParent.transform, false);
        var commandButton = commandObj.GetComponent<CommandButton>();
        commandButton.SetCommand(commandIndex, this);

        ButtonToCommand buttonToCommand;
        buttonToCommand.Button = commandButton;
        buttonToCommand.CommandIndex = commandIndex;
        _activeButtons.Add(buttonToCommand);
    }

    public void CommandButtonClicked(CommandButton button)
    {
        int commandIndex = button.CurrentCommandIndex;
        CommandControllerInstance.CommandClicked(commandIndex);
    }

    private struct ButtonToCommand
    {
        public CommandButton Button;
        public int CommandIndex;
    }
}