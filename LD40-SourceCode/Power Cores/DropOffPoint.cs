using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffPoint : MonoBehaviour
{
    public KeyCode dropoffKey = KeyCode.E;

    bool waitForInput = false;
    float openValue = 0f, openThreshold = 1f;

    void Start ()
    {
        waitForInput = false;
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.tag != "Player") return;

        openValue = 0f;
        if (GameController.Instance.HasCore)
        {
            UIController.Instance.ShowHoldMessage("drop off core", dropoffKey.ToString());
            waitForInput = true;
        }
    }

    void OnTriggerExit (Collider col)
    {
        if (col.tag != "Player") return;

        openValue = 0f;
        waitForInput = false;
    }

    void Update ()
    {
        if (waitForInput)
        {
            if (Input.GetKey(dropoffKey))
            {
                openValue += 1f * Time.deltaTime;
            }

            if (Input.GetKeyUp(dropoffKey) || Input.GetKeyDown(dropoffKey))
            {
                openValue = 0f;
            }

            if (openValue > openThreshold)
            {
                Drop();
            }
        }
    }

    void Drop ()
    {
        waitForInput = false;
        UIController.Instance.HideHoldMessage();
        GameController.Instance.CarryingCore(false);
        if (GameController.Instance.NumberOfCoresRemaining > 0)
        {
            UIController.Instance.ShowRemainingMessage();
        }
        else
        {
            GameController.Instance.EndGame(true);
        }
    }
}
