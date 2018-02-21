using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class MoveCube : MoveCubeBehavior
{
    private void Update()
    {
        // CLIENT
        if (!networkObject.IsServer)
        {
            transform.position = networkObject.position;
            transform.rotation = networkObject.rotation;
            return;
        }

        // SERVER
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0)
                              * Time.deltaTime
                              * 5f;
        transform.Rotate(Vector3.up, Time.deltaTime * 90f);

        networkObject.position = transform.position;
        networkObject.rotation = transform.rotation;
    }
}