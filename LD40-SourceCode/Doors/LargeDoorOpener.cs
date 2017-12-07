using StefanRichings;
using UnityEngine;

public class LargeDoorOpener : DoorOpener
{
    public GameObject[] doorParts;
    public float doorMoveTime = 1f;
    public float offsetMultiplier = 1f;

    Vector3 leftOpen, leftClosed, rightOpen, rightClosed;

    void Start()
    {
        if (doorParts.Length != 2)
        {
            Extensions.Error(gameObject, "Needs to be two door parts on this script!");
            return;
        }

        leftClosed = doorParts[0].transform.position;
        rightClosed = doorParts[1].transform.position;
        leftOpen = NewDoorPosition(doorParts[0], 6f * offsetMultiplier);
        rightOpen = NewDoorPosition(doorParts[1], -6f * offsetMultiplier);
    }

    protected override void OpenDoor ()
    {
        PlayAudio();

        UIController.Instance.HideHoldMessage();
        doorState = State.Opening;
        iTween.MoveTo(doorParts[0], leftOpen, doorMoveTime);
        iTween.MoveTo(doorParts[1], rightOpen, doorMoveTime);
        doorState = State.Open;
    }

    protected override void CloseDoor ()
    {
        PlayAudio();

        iTween.MoveTo(doorParts[0], leftClosed, doorMoveTime);
        iTween.MoveTo(doorParts[1], rightClosed, doorMoveTime);
        doorState = State.Closed;
    }
}
