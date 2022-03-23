using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class TailFollow : NetworkBehaviour
{
    [SyncVar]
    public Transform Head;
    public SnkRota snkRota;
    Collider2D Touch;
    public BodyControls BodyCtrl;

    //public ;
    void FindSnkRota()
    {
        snkRota = Head.GetComponent<SnkRota>();
        if (!snkRota.isLocalPlayer && snkRota.isClient) gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }


    void Update()
    {
        if (Head == null) { return; }
        if (!GameManager.Instance.Paused && snkRota == null) { FindSnkRota(); }
        if (snkRota == null)
        {
            Debug.Log("no SnakeRotation for Tail");
            return;
        }
        BodyCtrl = bodyCtrl;
        if (BodyCtrl != null)
        {
            autoMove();
            Touch = Physics2D.OverlapPoint(BodyCtrl.transform.GetChild(0).GetChild(1).position);
        }
        else
        {
            Debug.Log("No body for tail on Tail script");
        }
        if (Touch == null)
        {
            //Debug.Log("Touch nothing");
        }

    }

    public BodyControls bodyCtrl
    {
        get
        {
            if (snkRota.AllBody.Count > 0)
            {
                for (int i = 0; i < snkRota.AllBody.Count; i++)
                {
                    if (snkRota.AllBody[i] == null) { continue; }
                    BodyControls bodyCtrl = snkRota.AllBody[i].GetComponent<BodyControls>();
                    if (bodyCtrl != null) { return bodyCtrl; }
                }
            }
            return null;
        }
    }
    void autoMove()
    {
        transform.position = BodyCtrl.transform.GetChild(0).GetChild(1).position;
    }
}
