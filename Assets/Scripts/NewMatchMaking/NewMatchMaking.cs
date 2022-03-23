using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;

[System.Serializable]
public class MyServer
{
    public int ServerPort;
    public bool OpenServer;

    public MyServer(int ServerPort, bool OpenServer)
    {
        this.ServerPort = ServerPort;
        this.OpenServer = OpenServer;
    }
}
[System.Serializable]
public class TargetPort
{
    public int tarPort;
}

public class NewMatchMaking : NetworkBehaviour
{
    //public NetworkManager manager;
    public bool SearchMode;
    public int[] AllPorts;
    public List<MyServer> AllServer = new List<MyServer>();
    public kcp2k.KcpTransport transport;
    public GameObject netMan;
    // Start i  s called before the first frame update
    void Start()
    {
        if (isServer)
        {

            for (int i = AllPorts[0]; i <= AllPorts[1]; i++)
            {
                AllServer.Add(new MyServer(i, false));
            }

            SearchMode = true;
        } else
        {
            
        }
        
    }
    
    public int PlayersConnected;

    void Update()
    {
        if (isServer)
        {
            PlayersConnected = MatchMakingManager.PlayersNameConnected.Count;
            if (SearchMode && PlayersConnected >= 2)
            {
                ChecksServerState();
                Debug.Log("Player Finded");
                //AllServer[0].OpenServer = false;
                for (int i = 0; i < AllServer.Count; i++)
                {
                    if (AllServer[i].OpenServer)
                    {
                        List<string> PlyrSelected = new List<string> { MatchMakingManager.PlayersNameConnected[0], MatchMakingManager.PlayersNameConnected[1]};
                        Debug.Log("Function Called");
                       
                        MatchFindRpc(PlyrSelected, 7778 + i);
                        AllServer[i].OpenServer = false;
                        string ServerStatePath = Application.persistentDataPath + "/" + (7778 + i).ToString() + "_" + System.Environment.MachineName.ToString() + ".state";
                        System.IO.File.WriteAllText(ServerStatePath, "F");
                        SearchMode = false;
                        StartCoroutine(SearchOn());
                        break;
                    }
                }
               
            }
            
        }
    }

    void ChecksServerState()
    {
        for (int i = 0; i < AllServer.Count; i++)
        {
            string filePath = Application.persistentDataPath + "/" + AllServer[i].ServerPort.ToString() + "_" + System.Environment.MachineName.ToString() + ".state";
            if (System.IO.File.Exists(filePath))
            {
                string ServerConditionStr = System.IO.File.ReadAllText(filePath);
                if (ServerConditionStr == "F")
                {
                    AllServer[i].OpenServer = false;
                }
                else if (ServerConditionStr == "T")
                {
                    AllServer[i].OpenServer = true;
                }
            } else
            {
                AllServer[i].OpenServer = false;
            }

        }
    }

    IEnumerator SearchOn()
    {
        yield return new WaitForSeconds(1);
        SearchMode = true;
    }


    [ClientRpc]
    public void MatchFindRpc(List<string> targets, int MatchMakingPorts)
    {
        if (!targets.Contains(NetworkClient.localPlayer.name))
        {
            return;
        }



        // ------------------  SAVE PORT TO USE INTO JSON FILE  ------------------

        // File Path
        string filePath = Application.persistentDataPath + "/TargetPort.txt";
        Debug.Log(filePath);

        // PORT TO JSON CONVERSION
        
        string PortInStr = MatchMakingPorts.ToString();
        Debug.Log(PortInStr);

        // WRITE IN FILE
        System.IO.File.WriteAllText(filePath, PortInStr);


        StartCoroutine(ClientOff());
    }
    IEnumerator ClientOff()
    {
        yield return new WaitForSeconds(0.5f);
        NetworkManager.singleton.StopClient();
        Debug.Log("MatchFinded");
    }

}
