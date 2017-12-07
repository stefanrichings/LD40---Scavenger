using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCore : MonoBehaviour
{
    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    public void Take()
    {
        gameObject.SetActive(false);
        GameController.Instance.CarryingCore(true);
    }
}
