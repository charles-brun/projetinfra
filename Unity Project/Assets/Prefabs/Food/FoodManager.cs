using UnityEngine;
using Mirror;

public class FoodManager : NetworkBehaviour
{
    public GameObject FoodOriginal;
    [SyncVar]
    Transform Food;
    float hor = 15f;
    float ver = 6f;

    void Start()
    {
        if (isServer) { FoodCreate(); }
    }


    void Update()
    {

        if (Food == null)
        {
            if (isServer) { FoodCreate(); }
        }
    }

    void FoodCreate()
    {
        if (!isServer) { return; }
        if (FoodOriginal == null)
        {
            //Debug.Log("No Food Prefab");
            return;
        }

        Food = Instantiate(FoodOriginal, FoodCordinate, Quaternion.identity).transform;
        NetworkServer.Spawn(Food.gameObject);
    }

    Vector3 FoodCordinate
    {
        get
        {
            float FoodX = Random.Range(-hor, hor);
            float FoodY = Random.Range(-ver, ver);
            Vector3 FoodCord = new Vector3(FoodX, FoodY, 0);
            if (Physics2D.OverlapPoint(FoodCord))
            {
                return FoodCordinate;
            } else
            {
                return FoodCord;
            }
        }
    }
}
