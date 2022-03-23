using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTailState : MonoBehaviour
{

    void Update()
    {
        if (!GameManager.Instance.Paused)
        {
            gameObject.GetComponent<BodyControls>().enabled = true;
        }
    }
}
