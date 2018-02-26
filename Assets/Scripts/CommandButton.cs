using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandButton : CommandBehavior, IPointerClickHandler
{
    private int _commandIndex;
    [SerializeField] private TextMeshProUGUI _commandText;
    private NetworkedPlayer _np;

    public void OnPointerClick(PointerEventData eventData)
    {
        print("clicked");
        DoCommand();
    }

    public void Init(NetworkedPlayer np, int commandIndex, string commandText)
    {
        _np = np;
        _commandIndex = commandIndex;
        _commandText.text = commandText;
    }

    private void DoCommand()
    {
        _np.networkObject.SendRpc(CommandsBehavior.RPC_DO_COMMAND, Receivers.All, _commandIndex);
    }
}