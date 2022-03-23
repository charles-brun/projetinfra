using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BodyControls : NetworkBehaviour
{
    public SpriteRenderer MyColor;

    public Controls c;
    [SyncVar]
    public Transform BodyTarget;
    [SyncVar]
    public Transform Head;
    [SyncVar]
    public Transform TailPart;
    [SyncVar]
    public Transform BeforeBody;
    public Transform BodyEnd;
    public TailFollow Tail;

    public SnkRota snkRota;

    [SyncVar]
    public float LengthY;


    public float FinalLength;
    [SyncVar]
    public bool HaveToDestruct;
    [SyncVar]
    public bool CreationTime;
    [SyncVar]
    public bool HeadFollowing;
    [SyncVar]
    public bool ChangedTarget;
    [SyncVar]
    public Transform LiedConnector;
    [SyncVar]
    bool PlacedConnector = false;

    bool Started = false;
    [SyncVar]
    public float speed;

    

    private void Start()
    {
        Started = false;
    }
    [ClientRpc]
    public void RpcClientStart()
    {
        MyStart();
    }

    public void MyStart()
    {
        Started = true;
        if (Head == null)
        {
            Debug.Log("No Head At Start Body");
        }
        c = Head.GetComponent<Controls>();
        if (c == null)
        {
            Debug.Log("No Head At Start Body");
        }
        if (!c.isLocalPlayer && c.isClient) MyColor.color = Color.red;
        snkRota = Head.GetComponent<SnkRota>();
        if (snkRota.AllBody.Count == 1 && snkRota.AllBody[0] == null && isServer) { snkRota.AllBody[0] = gameObject; } 
        BodyEnd = transform.GetChild(0).GetChild(0);
        SetStartOnServer();
    }
    void SetStartOnServer()
    {
        if (!isServer) { return; }
        LengthY = transform.localScale.y;
        BodyTarget = Head;
        HeadFollowing = true;
        ChangedTarget = false;
    }

    public void FixedUpdate()
    {
        if (BeforeBody == null)
        {
            if (isServer) { CreationTime = false; }
        }
    }

    void Update()
    {
        if (GameManager.Instance.Paused) { return; }
        if (Head == null) { return; }
        if (!Started) { MyStart(); }
        if (c == null)
        {
            c = Head.GetComponent<Controls>();
        }
        if (TailPart == null && c != null && c.Tail != null)
        {
            if (isServer) { TailPart = c.Tail; }
            Tail = TailPart.GetComponent<TailFollow>();
        }
        if (Tail == null && TailPart != null)
        {
            Tail = TailPart.GetComponent<TailFollow>();
        }
        if (LiedConnector != null && !PlacedConnector && isServer)
        {
            PlacedConnector = true;
            LiedConnector.position = transform.position;
            BodyTarget = LiedConnector;
        }


        MvAndScaleGestor();
        HTDestruct();
        if (Tail != null && CreationTime && transform != null && (Tail.BodyCtrl == this || HeadFollowing))
        {
            if (transform.localScale.y < FinalLength)
            {
                BodyCreation();
            }
        }

    }



    void MvAndScaleGestor()
    {
        if (isServer && c.isBoosting) speed = 20;
        FinalLength = c.SnakeLength;
        if (HeadFollowing && transform != null && Head != null)
        {
            AutoMove();
        }
        /*if (!c.isLocalPlayer && HeadFollowing && LengthY > 0.22)
        {
            transform.localScale = new Vector3(transform.localScale.x, LengthY - 0.22f, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, LengthY, transform.localScale.z);
        }*/

        if (isServer) { transform.localScale = new Vector3(transform.localScale.x, LengthY, transform.localScale.z); }

        //ScaleAround(new Vector3(0.5f, LengthY, 1));
    }
    void AutoMove()
    {
        if (BodyTarget == null) { return; }
        transform.position = BodyTarget.position;
    }
    void BodyCreation()
    {
        if (isServer) { LengthY = Mathf.MoveTowards(LengthY, FinalLength, speed * Time.deltaTime); }
    }

    void HTDestruct()
    {
        if (HaveToDestruct)
        {
            if (isServer) { HeadFollowing = false; }
            if (Tail != null && Tail.BodyCtrl == this)
            {
                BodyDestruction();
            }

        }
    }

    void BodyDestruction()
    {
        if (isServer) 
        {
            LengthY = Mathf.MoveTowards(LengthY, 0, speed * Time.deltaTime);
            if (LengthY <= 0.25f && HaveToDestruct)
            {
                //Debug.Log("destroy body");
                LengthDestroy();
                //this.enabled = false;
            }
        }
        
    }

    

    void LengthDestroy()
    {
        if (snkRota.AllBody.Contains(gameObject))
        {
            snkRota.AllBody.RemoveAt(0);
        }
        NetworkServer.Destroy(LiedConnector.gameObject);
        NetworkServer.Destroy(gameObject);

    }




}
