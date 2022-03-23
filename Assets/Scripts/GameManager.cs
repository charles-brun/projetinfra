using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : NetworkBehaviour
{
    private static GameManager _instance;
    public GameObject Net;
    public GameObject FoodMan;
    public GameObject[] CountDownPrefabs;
    public bool isStarting;
    NetworkManager manager;
    public int MyPort;
    public static GameManager Instance { get { return _instance; } }

    [SyncVar]
    public int Plyr_Nb;

    [SyncVar]
    public int InLifePlyr;


    [SyncVar]
    public bool Paused;
    [SyncVar]
    public bool GameStart = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    private void Start()
    {
        Application.targetFrameRate = 144;
        isStarting = false;
        manager = Net.GetComponent<NetworkManager>();
        if (isServer) 
        { 
            Plyr_Nb = 0;
            GameStart = false;
        }
    }
    private void Update()
    {
        if (!isServer) { return; }
        if (Plyr_Nb < 2)
        {
            Paused = true;
            FoodMan.SetActive(false);
            GameStart = false;
        }
        if (Plyr_Nb >= 2)
        {
            if (GameStart)
            {
                Paused = false;
                FoodMan.SetActive(true);
            } else
            {
                if (isStarting) { return; }
                isStarting = true;
                InstanteAllPlyrRpc();
                StartCoroutine(StartTheGame());
            }

        }
    }
    [ClientRpc]
    void InstanteAllPlyrRpc()
    {
        NetworkClient.AddPlayer();
        Debug.Log("NEW PLAYER");
        
    }

    IEnumerator StartTheGame()
    {
        ActiveNumber(2);
        yield return new WaitForSeconds(1);
        ActiveNumber(1);
        yield return new WaitForSeconds(1);
        ActiveNumber(0);
        yield return new WaitForSeconds(1);
        GameStart = true;
        ActiveNumber(-1);
    }

    public void DisconnectAll()
    {
        if (!isServer) { return; }
        Debug.Log("DisconnectAll!");
        RestartServer();
    }

    void RestartServer()
    {
        Debug.Log("StopServer!");
        manager.StopServer();
        gameObject.SetActive(true);
        StartCoroutine(StServer());
    }

    IEnumerator StServer()
    {
        gameObject.SetActive(true);
        string ServerStatePath = Application.persistentDataPath + "/" + MyPort.ToString() + "_" + System.Environment.MachineName.ToString() + ".state";
        File.WriteAllText(ServerStatePath, "T");
        yield return new WaitForSeconds(1f);
        Plyr_Nb = 0;
        isStarting = false;
        GameStart = false;

        manager.StartServer();
        Debug.Log("StartServer!");

    }

    public void ActiveNumber(int nb)
    {
        GameObject counter = GameObject.FindGameObjectWithTag("CountDown");
        if (counter != null)
        {
            NetworkServer.Destroy(counter);
        }
        if (nb < 0) { return; }
        GameObject NewCounter = Instantiate(CountDownPrefabs[nb]);
        NetworkServer.Spawn(NewCounter);  
    }

}