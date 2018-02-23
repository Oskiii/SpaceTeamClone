using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

public class CommandList : MonoBehaviour
{
    private NetworkedPlayer np;
    [SerializeField] private CommandButton _commandButtonPrefab;
    [SerializeField] private RectTransform _commandButtonParent;

    private void Awake()
    {
        np = GetComponent<NetworkedPlayer>();
        np.NetworkStartEvent += NetworkStart;
        np.DoCommandEvent += DoCommand;
    }

    private void NewCommandButton(){
        GameObject obj = Instantiate(_commandButtonPrefab.gameObject, _commandButtonParent);
        CommandButton button = obj.GetComponent<CommandButton>();
        button.Init(np);
    }

    private void NetworkStart()
    {

    }

    private void DoCommand(RpcArgs args){

    }

    private void CompleteCommand(){
        // Send RPC to all that this player's Command was completed
        np.networkObject.SendRpc (CommandsBehavior.RPC_COMMAND_COMPLETED, Receivers.All);
    }
}