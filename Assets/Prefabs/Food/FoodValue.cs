using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FoodValue : NetworkBehaviour
{
    public float FoodNutrition;
    Collider2D col;
    Controls c;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("touché!");
        col = collision;
        if (col != null)
        {
            c = col.transform.root.GetComponent<Controls>();
            if (c != null)
            {
                if (isServer) { AddLen(); }
            }

        }
    }

    void AddLen()
    {
        if (c == null)
        {
            return;
        }
        c.SnakeLength += FoodNutrition;
        NetworkServer.Destroy(gameObject);
    }
}
