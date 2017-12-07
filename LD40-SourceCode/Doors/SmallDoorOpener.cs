using StefanRichings;
using UnityEngine;

public class SmallDoorOpener : DoorOpener
{
    public GameObject door;
    public float doorMoveTime = 1f;

    Vector3 doorOpen, doorClosed;

    void Start()
    {
        if (door == null)
        {
            Extensions.Error(gameObject, "This script doesn't have a door!");
        }

        doorClosed = door.transform.position;
        doorOpen = NewDoorPosition(door, 3.5f);
    }

    protected override void OpenDoor ()
    {
        PlayAudio();

        UIController.Instance.HideHoldMessage();
        doorState = State.Opening;
        iTween.MoveTo(door, doorOpen, doorMoveTime);
        doorState = State.Open;
    }

    protected override void CloseDoor ()
    {
        PlayAudio();

        iTween.MoveTo(door, doorClosed, doorMoveTime);
        doorState = State.Closed;
    }
}
