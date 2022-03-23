using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class HeadChecker : NetworkBehaviour
{
    public Controls c;
    public Transform HeadDetector1;
    public Transform HeadDetector2;
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (!c.isServer) { return; }
        if (collision.name != "Food(Clone)" && collision.transform != transform.root)
        {

            //if (c.isClient) c.CmdDestruct("Death with " + collision.name + " at " + collision.transform.position + " of " + transform.position);
            //Debug.Log("Death with " + collision.name + " at " + collision.transform.position + " of " + transform.position);
            Debug.Log("Death with " + transform.name + collision.name);
            c.AllSnakeDestruct();

        }
    }*/


    private void Update()
    {
        if (!isServer) { return; }
        if (Physics2D.OverlapArea(HeadDetector1.position, HeadDetector2.position))
        {
            GameObject collision = Physics2D.OverlapArea(HeadDetector1.position, HeadDetector2.position).gameObject;
            if (collision.name != "Food(Clone)")
            {
                Debug.Log("Collision Between " + transform.name + " " + collision.name);
                c.AllSnakeDestruct();
            }
        }
    }

}
