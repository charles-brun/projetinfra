using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class OnClientConn : NetworkManager
{

    public kcp2k.KcpTransport tr_port;

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log(Application.dataPath);
    }



    public override void OnStopServer()
    {
        base.OnStopServer();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("Player Connected");
        GameManager.Instance.Plyr_Nb++;
        GameManager.Instance.InLifePlyr++;
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("Player Disconnected");
        GameManager.Instance.Plyr_Nb--;
        GameManager.Instance.InLifePlyr--;
        if (GameManager.Instance.Plyr_Nb <= 1) { GameManager.Instance.DisconnectAll(); }
        
    }

    public override void OnStartHost()
    {
        GameManager.Instance.Plyr_Nb++;
    }
}
