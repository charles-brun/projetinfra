using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerPort : NetworkBehaviour
{
    public int myPort;
    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            myPort = 7777;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void ServDeco()
    {
        if (isServer)
        {
            int PlayersConnected = NetworkServer.connections.Count;
            if (PlayersConnected >= 1)
            {

            }
        }
    }
    [ClientRpc]
    void ClientDecoRpc()
    {

    }
}
