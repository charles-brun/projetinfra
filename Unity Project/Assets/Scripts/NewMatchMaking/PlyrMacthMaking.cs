using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlyrMacthMaking : NetworkBehaviour
{

    public NewMatchMaking MM;

    void Start()
    {
        MM = GameObject.Find("OnCamera").GetComponent<NewMatchMaking>();
    }

    /*[Command]
    public void CmdAdrs()
    {
        for (int i = 0; i < MM.AllConnections.Count; i++)
        {
            AdrsRpc(MM.AllConnections[i].address, MM.AllConnections[i].connectionId);
        }
    }

    [ClientRpc]
    public void AdrsRpc(string adrs, int iddd)
    {
        Debug.Log("My Adress is " + NetworkClient.connection.address + " from server is " + adrs);
        Debug.Log("My id is " + NetworkClient.connection.connectionId + " from server is " + iddd);
        
    }
    */
    public override void OnStartClient()
    {
        base.OnStopClient();
        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        MatchMakingManager.RegisterPlayer(netId, this);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        MatchMakingManager.RegisterPlayer(netId, this);
    }

    private void OnDisable()
    {
        MatchMakingManager.UnregisterPlayer(transform.name);
    }

    
}
