using BeardedManStudios.Forge.Networking.Generated;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandButton : CommandBehavior, IPointerClickHandler
{
    private CommandList _commandList;
    [SerializeField] private TextMeshProUGUI _commandText;
    public int CurrentCommandIndex { get; private set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("clicked");
        _commandList.CommandButtonClicked(this);
    }

    public void SetCommand(int commandIndex, CommandList parent)
    {
        //_commandText.text = NetworkedPlayer.CommandsStrings[commandIndex];
        //_commandList = parent;
        //CurrentCommandIndex = commandIndex;
    }
}