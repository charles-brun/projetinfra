using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerInit : NetworkManager
{

    public int MyPort;
    public ServerPort PortScript;
    public override void OnServerConnect(NetworkConnection conn)
    {
        MyPort++;
        PortScript.myPort = MyPort;
        Debug.Log("Player Connected");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
