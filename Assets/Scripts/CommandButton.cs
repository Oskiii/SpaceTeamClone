using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandButton : CommandBehavior, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _commandText;
    private NetworkedPlayer _np;
    public int CurrentCommandIndex { get; private set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("clicked");
        DoCommand();
    }

    public void Init(NetworkedPlayer np, int commandId, string commandText)
    {
        _np = np;
        _commandText.text = commandText;
    }

    private void DoCommand()
    {
        _np.networkObject.SendRpc(CommandsBehavior.RPC_DO_COMMAND, Receivers.All, 0);
    }
}