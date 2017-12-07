using StefanRichings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCoreHolder : MonoBehaviour
{
    public PowerCore powerCore;
    public Transform ring;
    public GameObject energyField;
    public Light pointLight;
    public KeyCode collectKey = KeyCode.E;

    float rotateSpeed = 30f, openValue = 0f, openThreshold = 1f;
    bool waitForInput = false, hasCore = true;

    void Start ()
    {
        if (powerCore == null)
        {
            Extensions.Error(gameObject, "Missing the power core");
            return;
        }

        if (ring == null)
        {
            Extensions.Error(gameObject, "Missing the ring");
            return;
        }

        if (energyField == null)
        {
            Extensions.Error(gameObject, "Missing the energy field");
            return;
        }

        if (pointLight == null)
        {
            pointLight = GetComponentInChildren<Light>();
        }

        waitForInput = false;
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.tag != "Player") return;

        openValue = 0f;
        if (!GameController.Instance.HasCore && hasCore)
        {
            UIController.Instance.ShowHoldMessage("pick up core", collectKey.ToString());
            waitForInput = true;
        }
    }

    void OnTriggerExit (Collider col)
    {
        if (col.tag != "Player") return;

        if (UIController.Instance.CurrentMessage == "pick up core")
        {
            UIController.Instance.HideHoldMessage();
        }
        openValue = 0f;
        waitForInput = false;
    }

    void Update ()
    {
        if (rotateSpeed > 0)
        {
            ring.RotateAround(powerCore.Position, Vector3.right, rotateSpeed * Time.deltaTime);
            ring.RotateAround(powerCore.Position, Vector3.back, rotateSpeed * Time.deltaTime);
            ring.RotateAround(powerCore.Position, Vector3.up, rotateSpeed * Time.deltaTime);
        }

        if (waitForInput)
        {
            if (Input.GetKey(collectKey))
            {
                openValue += 1f * Time.deltaTime;
            }

            if (Input.GetKeyUp(collectKey) || Input.GetKeyDown(collectKey))
            {
                openValue = 0f;
            }

            if (openValue > openThreshold)
            {
                TakeCore();
            }
        }
    }

    public void TakeCore ()
    {
        UIController.Instance.HideHoldMessage();
        GetComponent<AudioSource>().Play();
        energyField.SetActive(false);
        StartCoroutine(Shutdown());
        waitForInput = false;
        hasCore = false;
        powerCore.Take();
    }

    IEnumerator Shutdown ()
    {
        float divisor = 30f;
        float rotateDivisor = rotateSpeed / divisor;
        float lightDivisor = pointLight.intensity / divisor;

        while (rotateSpeed > 0 && pointLight.intensity > 0)
        {
            rotateSpeed -= rotateDivisor;
            pointLight.intensity -= lightDivisor;
            yield return null;
        }
    }
}
