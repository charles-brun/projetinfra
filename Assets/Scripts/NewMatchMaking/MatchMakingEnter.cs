using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMakingEnter : MonoBehaviour
{
    public GameObject netMan;
    public bool IAmTheServer;
    void Start()
    {
        if (netMan != null)
        {
            if (IAmTheServer)
            {
                netMan.GetComponent<Mirror.NetworkManager>().StartServer();
            }
            else
            {
                netMan.GetComponent<Mirror.NetworkManager>().networkAddress = "35.181.61.44";
                netMan.GetComponent<Mirror.NetworkManager>().StartClient();
            }
        } else
        {
            Debug.Log("No MatchMaking NetworkManager FIND!!");
        }
    }
}
