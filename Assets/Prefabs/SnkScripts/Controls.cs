using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public enum CharacterDirection
{
    Verticale,
    Horizontal,
}

public struct SnkDirection
{
    public CharacterDirection _Direction;
    public Vector3 _Rotate;

    public SnkDirection(CharacterDirection _Direction, Vector3 _Rotate)
    {
        this._Direction = _Direction;
        this._Rotate = _Rotate;
    }
}

public class Controls : NetworkBehaviour
{
    [Header("Body Parts")]
    public GameObject BodyPrefabs;
    public GameObject TailPrefabs;

    public SnkRota snkRota;

    [Header("Snake Parts")]
    [SyncVar]
    public Transform Body;
    [SyncVar]
    public Transform Tail;


    [SyncVar]
    public SnkDirection SnakeDirection;
    public GameObject prefab;
    public Transform GoForw;
    [SyncVar]
    public float speed;


    [Header(" Snake Propriety")]
    [SyncVar]
    public float SnakeLength;
    [SyncVar]
    public float SnkPartsLen;

    public bool GameStarted = false;



    // ABILTY
    // 
    [SyncVar]
    public bool isBoosting;


    //
    //
    private void Start()
    {
        if (!isLocalPlayer && isClient) gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void MyStart()
    {
        GameStarted = true;
        if (isServer) SnakeLength = 4;
        if (isServer) { PlyrInit(); }
        if (isServer) { snkRota.CreateBody(transform.rotation.eulerAngles); }
        if (isLocalPlayer) transform.tag = "Player";
    }
    
    void PlyrInit()
    {
        //FoodManager.Instance.gameObject.SetActive(true);
        // CREATE BODY AND TAIL

        Vector3 TailPos = new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z);
        GameObject tlTemp = Instantiate(TailPrefabs, TailPos, transform.rotation);

        //SPAWN
        
        Tail = tlTemp.transform;
        tlTemp.transform.GetComponent<TailFollow>().Head = transform;

        //OTHER SET UP
        snkRota.TailPart = tlTemp.transform;
        tlTemp.transform.GetComponent<TailFollow>().enabled = true;
        NetworkServer.Spawn(tlTemp, gameObject);
        RpcSet(tlTemp.transform);
    }
    [ClientRpc]
    void RpcSet(Transform NewTail)
    {
        if (NewTail == null)
        {
            Debug.Log("Tail not passed in the void");
        }
        /*NewBody.transform.GetComponent<BodyControls>().Head = transform;
        NewTail.transform.GetComponent<TailFollow>().Head = transform;*/
        NewTail.GetComponent<TailFollow>().enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Paused) { return; }
        if (!GameStarted) { MyStart(); }
        if (Tail != null && Tail.GetComponent<TailFollow>() != null && !Tail.GetComponent<TailFollow>().enabled)
        {
            Tail.GetComponent<TailFollow>().enabled = true;
        }
        AutoMove();
        if (isServer) { CheckSnkLen(); } else { CmdCheckSnk(); }

        // CONTROLS
        if (!isLocalPlayer || isBoosting) { return; }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z))
        {
            CmdChangePlyrDir(CharacterDirection.Verticale, new Vector3(0, 0, 0));
        } else
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Q))
        {
            CmdChangePlyrDir(CharacterDirection.Horizontal, new Vector3(0, 0, 90));
        } else
        if (Input.GetKeyDown(KeyCode.S))
        {
            CmdChangePlyrDir(CharacterDirection.Verticale, new Vector3(0, 0, 180));
        } else
        if (Input.GetKeyDown(KeyCode.D))
        {
            CmdChangePlyrDir(CharacterDirection.Horizontal, new Vector3(0, 0, 270));
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isClient) { CmdSetSpeed(); }
            //AddSnkLenServerRpc();
        }



        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            CmdCreatePrefab();
        }*/
    }
    [Command]
    void CmdSetSpeed()
    {
        StartCoroutine(DashAbility());
        /*
        if (!isBoosting)
        {
            
            speed = 20;
            isBoosting = true;
        } else
        {
            speed = 5;
            isBoosting = false;
        }
        */
    }

    IEnumerator DashAbility()
    {
        speed = 40;
        isBoosting = true;
        yield return new WaitForSeconds(0.1f);
        speed = 5;
        isBoosting = false;
    }

    [Command]
    void AddSnkLenServerRpc()
    {
        SnakeLength++;
    }
    [Command]
    void CmdChangePlyrDir(CharacterDirection newDir, Vector3 RotateTo)
    {
        if ((RotateTo == new Vector3(0, 0, 0) || RotateTo == new Vector3(0, 0, 180)) && SnakeDirection._Direction == CharacterDirection.Verticale)
        {
            return;
        }
        if ((RotateTo == new Vector3(0, 0, 90) || RotateTo == new Vector3(0, 0, 270)) && SnakeDirection._Direction == CharacterDirection.Horizontal)
        {
            return;
        }
        if (transform.rotation.eulerAngles == RotateTo) { return; }
        transform.rotation = Quaternion.Euler(RotateTo);
        SnakeDirection = new SnkDirection(newDir, RotateTo);
        OffDestruct();
        snkRota.CreateConnection(transform.rotation.eulerAngles);
        //snkRota.CmdCreateBody(RotateTo, transform.position);

    }

    void OffDestruct()
    {
        if (Body != null)
        {
            Body.GetComponent<BodyControls>().HaveToDestruct = true;
        }
        else
        {
            Debug.Log("No Body in Controls");
        }
    }


    public void AllSnakeDestruct()
    {
        snkRota.FullDestruction();

    }

    void AutoMove()
    {
        if (!isServer) { return; }
        transform.position = Vector3.MoveTowards(transform.position, GoForw.position, speed * Time.deltaTime);

    }

    [Command]
    void CmdCreatePrefab()
    {
        //Destroy(snkRota.AllBody[0]);
        
        /*NetworkServer.Destroy(snkRota.AllBody[0]);
        snkRota.AllBody.RemoveAt(0);*/

    }



    [Command(requiresAuthority = false)]
    void CmdCheckSnk()
    {
        CheckSnkLen();
    }
    void CheckSnkLen()
    {
        SnkPartsLen = 0;
        for (int i = snkRota.AllBody.Count - 1; i >= 0; i--)
        {
            if (snkRota.AllBody[i] == null) { return; }
            SnkPartsLen += snkRota.AllBody[i].transform.localScale.y;
        }
        if (snkRota.AllBody.Count <= 0) { return; }
        if (snkRota.AllBody[0].GetComponent<BodyControls>() == null) { return; }

        if (SnkPartsLen <= SnakeLength - 0.24f)
        {
            snkRota.AllBody[0].GetComponent<BodyControls>().CreationTime = true;
        }
        /*
        if (SnkPartsLen > SnakeLength + 0.02f)
        {
            if (isBoosting)
            {
                snkRota.AllBody[0].GetComponent<BodyControls>().speed = 0.75f;
            } else
            {
                snkRota.AllBody[0].GetComponent<BodyControls>().speed = 10;
            }
            
        }
        else
        {
            if (isBoosting)
            {
                snkRota.AllBody[0].GetComponent<BodyControls>().speed = 0.75f;
            }
            else
            {
                snkRota.AllBody[0].GetComponent<BodyControls>().speed = 5;

            }
        }*/
    }

}

