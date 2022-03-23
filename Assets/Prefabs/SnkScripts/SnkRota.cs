using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SnkRota : NetworkBehaviour
{
    public Controls c;
    public float DifScale;
    [Header("Needed Head")]
    public GameObject BodyPrefabs;
    public GameObject ConnectionPrefabs;

    [Header("Snake Parts")]

    readonly public SyncList<GameObject> AllBody = new SyncList<GameObject>();
    readonly public SyncList<GameObject> AllConnection = new SyncList<GameObject>();

    [SyncVar]
    public Transform TailPart;

    public bool ChangeColor = false;

    // Start is called before the first frame update
    void Start()
    {
        if (isClient && !isLocalPlayer)
        {
            ChangeColor = true;
        }
    }

    public void CreateConnection(Vector3 DirRotate)
    {
        // CREATION OF THE CONNECTION

        GameObject objconn = Instantiate(ConnectionPrefabs, transform.position, Quaternion.identity);
        NetworkServer.Spawn(objconn, gameObject);
        GameObject oldBody;
        if (AllBody.Count > 0)
        {
            oldBody = AllBody[AllBody.Count - 1];
            oldBody.GetComponent<BodyControls>().LiedConnector = objconn.transform;
        } else
        {
            Debug.Log("ALL BODY < 0... WAIT WHAT???");
        }

        RpcSetConnColor(objconn.transform);
        AllConnection.Add(objconn.gameObject);
        CreateBody(DirRotate);
    }
    [ClientRpc]
    void RpcSetConnColor(Transform objConn)
    {
        if (!isLocalPlayer) objConn.GetComponent<SpriteRenderer>().color = Color.red;
    }

    /*[Command]
    public void CmdToServerBody(Vector3 DirRotate, Vector3 BodyPos)
    {
        CreateBody(DirRotate, BodyPos);
    }*/
    public void CreateBody(Vector3 DirRotate)
    {

        Transform CurrentBody = null;
        if (AllBody.Count > 0)
        {
            CurrentBody = AllBody[AllBody.Count - 1].transform;
        }
        GameObject NewBody;
        if (AllConnection.Count > 0)
        {
            NewBody = Instantiate(BodyPrefabs, AllConnection[AllConnection.Count-1].transform.position, Quaternion.Euler(DirRotate));

        }
        else
        {
            NewBody = Instantiate(BodyPrefabs, transform.position, Quaternion.Euler(DirRotate));

        }
        BodyControls BdControls = NewBody.GetComponent<BodyControls>();
        NetworkServer.Spawn(NewBody, gameObject);
        AllBody.Add(NewBody);


        // SET UP
        if (CurrentBody != null)
        {
            CurrentBody.GetComponent<BodyControls>().HeadFollowing = false;
            BdControls.BeforeBody = CurrentBody;

        }
        BdControls.Head = transform;
        BdControls.transform.localScale = new Vector3(BdControls.transform.localScale.x, 0.1f, BdControls.transform.localScale.z);
        c.Body = NewBody.transform;
        BdControls.HaveToDestruct = false;
        BdControls.CreationTime = true;
        BdControls.TailPart = TailPart;
        BdControls.enabled = true;
        //SetScaleSnk();



        RpcSetActive(BdControls.transform);
    }


    [ClientRpc]
    public void RpcSetActive(Transform NewBody)
    {
        NewBody.GetComponent<BodyControls>().enabled = true;
        if (!isLocalPlayer && isClient) NewBody.GetComponent<BodyControls>().MyColor.color = Color.red;
        //SetScaleSnk();
        //GetComponent<NetworkTransform>().enabled = false;
        //CmdSnkChangePos();
    }

    /*[Command]
    void CmdSnkChangePos()
    {
        if (AllConnection.Count > 0) { transform.position = AllConnection[AllConnection.Count - 1].transform.position; }
    }*/

    public void FullDestruction()
    {
        if (!isServer) { return; }

        for (int i = 0; i < AllConnection.Count; i++)
        {
            if (AllConnection[i] == null)
            {
                AllConnection.RemoveAt(i);
            } else
            {
                NetworkServer.Destroy(AllConnection[i]);
            }
        }

        for (int i = 0; i < AllBody.Count; i++)
        {
            if (AllBody[i] == null)
            {
                AllBody.RemoveAt(i);
            }
            else
            {
                NetworkServer.Destroy(AllBody[i]);
            }
        }
        NetworkServer.Destroy(TailPart.gameObject);
        NetworkServer.Destroy(gameObject);
        GameManager.Instance.InLifePlyr--;
        if (GameManager.Instance.InLifePlyr <= 1) { GameManager.Instance.DisconnectAll(); }
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.Paused) { return; }

        if (!isServer) { return; }
        for (int i = 0; i < AllConnection.Count; i++)
        {
            if (AllConnection[i] == null)
            {
                AllConnection.RemoveAt(i);
            }
        }
        for (int i = 0; i < AllBody.Count; i++)
        {
            if (AllBody[i] == null)
            {
                AllBody.RemoveAt(i);
            }
        }
        if (isServer)
        {
            for (int i = 0; i < AllBody.Count; i++)
            {
                // SET UP
                if (!AllBody[i].GetComponent<BodyControls>().enabled) { continue; }
                if (AllBody[i].GetComponent<BodyControls>().Head == null)
                {
                    AllBody[i].GetComponent<BodyControls>().Head = transform;
                    AllBody[i].GetComponent<BodyControls>().MyStart();
                    AllBody[i].GetComponent<BodyControls>().RpcClientStart();
                }
                

                // DESTROY
                if (AllBody[i].GetComponent<BodyControls>().HeadFollowing && i != AllBody.Count - 1)
                {
                    NetworkServer.Destroy(AllBody[i]);
                }
            }

        }
    }

}
