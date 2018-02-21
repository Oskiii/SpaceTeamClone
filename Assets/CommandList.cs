using UnityEngine;

public class CommandList : MonoBehaviour
{
    private NetworkedPlayer np;

    private void Awake()
    {
        np = GetComponent<NetworkedPlayer>();
        np.NetworkStartEvent += NetworkStart;
    }

    private void NetworkStart()
    {
    }
}