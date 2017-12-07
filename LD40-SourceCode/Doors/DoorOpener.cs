using StefanRichings;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorOpener : MonoBehaviour
{
    public AudioClip doorOpenSound;
    public KeyCode doorOpenKey = KeyCode.E;
    public float openThreshold = 2f;
    public Axis movementAxis;
    public bool requireInput = false;

    protected float openValue;
    protected enum State
    {
        Closed,
        Opening,
        Open
    }
    protected State doorState = State.Closed;
    protected bool waitForInput = false;

    void Start()
    {
        waitForInput = false;
    }

    protected void OnTriggerEnter(Collider col)
    {
        openValue = 0f;
        waitForInput = true;
        if (requireInput)
        {
            UIController.Instance.ShowHoldMessage("open door", doorOpenKey.ToString());
        }
    }

    protected void OnTriggerExit(Collider col)
    {
        if (UIController.Instance.CurrentMessage == "open door")
        {
            UIController.Instance.HideHoldMessage();
        }
        openValue = 0f;
        waitForInput = false;

        if (doorState == State.Open || doorState == State.Opening)
        {
            CloseDoor();
        }
    }

    protected void PlayAudio ()
    {
        if (doorOpenSound != null)
        {
            var a = GetComponent<AudioSource>();
            a.clip = doorOpenSound;
            a.Play();
        }
    }

    protected void Update()
    {
        if (waitForInput)
        {
            if (requireInput)
            {
                if (Input.GetKey(doorOpenKey))
                {
                    openValue += 1f * Time.deltaTime;
                }

                if (Input.GetKeyUp(doorOpenKey) || Input.GetKeyDown(doorOpenKey))
                {
                    openValue = 0f;
                }

                if (openValue > openThreshold)
                {
                    if (doorState == State.Closed)
                    {
                        OpenDoor();
                    }
                }
            }
            else
            {
                if (doorState == State.Closed)
                {
                    OpenDoor();
                }
            }
        }
    }

    protected virtual void OpenDoor()
    {
        doorState = State.Opening;
        StartCoroutine(WaitForDoor());
        doorState = State.Open;
    }

    protected virtual void CloseDoor()
    {
        StartCoroutine(WaitForDoor());
        doorState = State.Closed;
    }

    protected virtual IEnumerator WaitForDoor()
    {
        yield return new WaitForSeconds(1f);
    }

    protected Vector3 NewDoorPosition (GameObject g, float offset)
    {
        Vector3 newPos = g.transform.position;
        switch (movementAxis)
        {
            case Axis.X:
                newPos.x += offset;
                break;
            case Axis.Y:
                newPos.y += offset;
                break;
            case Axis.Z:
                newPos.z += offset;
                break;
        }
        return newPos;
    }
}
