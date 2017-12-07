using Pathfinding;
using UnityEngine;

public class SpiderHunt : MonoBehaviour
{
    // The point to move to
    public Transform targetPosition;
    private Seeker seeker;
    private CharacterController controller;
    // The calculated path
    public Path path;
    // The AI's speed in meters per second
    public float speed = 2;
    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;
    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;
    // How often to recalculate the path (in seconds)
    public float repathRate = 0.5f;
    private float lastRepath = -9999;

    Spider spider;

    public void Start ()
    {
        spider = GetComponent<Spider>();
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
    }

    public void OnPathComplete (Path p)
    {
        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    public void Update ()
    {
        if (spider.Alive)
        {
            targetPosition = GameController.Instance.Player;
            transform.LookAt(targetPosition);

            if (Time.time - lastRepath > repathRate && seeker.IsDone())
            {
                lastRepath = Time.time + Random.value * repathRate * 0.5f;
                // Start a new path to the targetPosition, call the the OnPathComplete function
                // when the path has been calculated (which may take a few frames depending on the complexity)
                seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
            }
            if (path == null)
            {
                // We have no path to follow yet, so don't do anything
                return;
            }
            if (currentWaypoint > path.vectorPath.Count) return;
            if (currentWaypoint == path.vectorPath.Count)
            {
                currentWaypoint++;
                return;
            }
            // Direction to the next waypoint
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            dir *= speed;
            // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
            controller.SimpleMove(dir);
            // The commented line is equivalent to the one below, but the one that is used
            // is slightly faster since it does not have to calculate a square root
            //if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
            if ((transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance * nextWaypointDistance)
            {
                currentWaypoint++;
                return;
            }
        }
        else
        {
            if (controller != null)
            {
                Destroy(controller);
            }
        }
    }
}
