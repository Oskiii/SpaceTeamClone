using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandButton : CommandBehavior, IPointerClickHandler {
    private CommandList _commandList;
    private NetworkedPlayer _np;
    [SerializeField] private TextMeshProUGUI _commandText;
    public int CurrentCommandIndex { get; private set; }

    public void Init (NetworkedPlayer np) {
        _np = np;
    }

    public void OnPointerClick (PointerEventData eventData) {
        print ("clicked");
        DoCommand();
    }

    private void DoCommand () {
        _np.networkObject.SendRpc (CommandsBehavior.RPC_DO_COMMAND, Receivers.All, 0);
    }

    public void SetCommand (int commandIndex, CommandList parent) {
        //_commandText.text = NetworkedPlayer.CommandsStrings[commandIndex];
        //_commandList = parent;
        //CurrentCommandIndex = commandIndex;
    }
}