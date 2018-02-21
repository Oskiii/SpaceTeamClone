using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class Guy : GuyBehavior
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            networkObject.SendRpc(RPC_MOVE_UP, Receivers.AllBuffered);
    }

    public override void MoveUp(RpcArgs args)
    {
        transform.position += Vector3.up;
    }
}