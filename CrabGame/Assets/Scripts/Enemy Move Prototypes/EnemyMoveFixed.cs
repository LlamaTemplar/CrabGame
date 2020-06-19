using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMoveFixed : MonoBehaviour
{
    public Transform target;
    public bool toggleRotate = false;

    public float moveSpeed = 400f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint;
    public bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public float rotationSpeed = 0.5f;
    public int facingingRange = 8;
    private Transform startingPosition;
    private Vector3 vectorToTarget;
    private float angle;
    private Quaternion direction;

    public Transform pathbeginning;
    public Transform pathDestination;
    public bool once = false;
    public float switchDelay;
    public float switchStartDelay = 4f;
    public bool switched = false;
    public bool playerIsNear = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get componenets
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform;

        switchDelay = switchStartDelay;

        // This is not used, but kept for understanding the Pathfinding code
        //InvokeRepeating("UpdatePath", 0f, .5f);
    }

    // This is not used, but kept for understanding the Pathfinding code
    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    // Create Waypoint and path
    void OnPathComplete(Path p)
    {
        // If we don't have an error...
        if (!p.error)
        {
            // Generate Path (Not clear on how that is done)
            path = p;
            // Start waypoint at begining
            currentWaypoint = 0;
        }
        else// If we have error..
        {
            // Print Error
            print("We have a Path Error");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If Enemy has reahced end of path and the player is not near, start delay timer, before switcing path
        if (reachedEndOfPath && playerIsNear == false)
        {
            // COunt down Delay timer
            switchDelay -= Time.deltaTime;
        }

        // When Delay is at 0...
        if (switchDelay <= 0)
        {
            // Reset Switch delay
            switchDelay = switchStartDelay;
            // Once becomes false, so we start a new path
            once = false;
            // Change path
            switched = !switched;
        }

        // Check if StartPath is called once (If it is called more than once, the AI follows the path very slowly)
        if (once == false)
        {
            // If we haven't switched the path yet...
            if (switched == false)
            {
                // Start follow path to Destination
                seeker.StartPath(rb.position, pathDestination.position, OnPathComplete);
                // Once becomes true
                once = true;
            }
            else // If we did switch the path...
            {
                // Start follow the path back to starting position
                seeker.StartPath(rb.position, pathbeginning.position, OnPathComplete);
                // Once becomes true
                once = true;
            }
        }

        // If the player is within a facing range of the enemy...
        if (Vector2.Distance(transform.position, target.position) <= facingingRange && toggleRotate == false)
        {
            // Player is near is true
            playerIsNear = true;
        }
        else
        {
            playerIsNear = false;
        }

        // If Player is near...
        if (playerIsNear)
        {
            // Find the vector pointing from our position to the target
            vectorToTarget = target.position - transform.position;
            // Calculate the angle from the point
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            // Create the rotation we need to be in to look at the target
            direction = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            // Rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * rotationSpeed);
        }

        if (toggleRotate) // If toggleRotate is true, rotate facing Player (Does not matter if player is near or not)
        {
            // Find the vector pointing from our position to the target
            vectorToTarget = target.position - transform.position;
            // Calculate the angle from the point
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            // Create the rotation we need to be in to look at the target
            direction = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            // Rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * rotationSpeed);
        }
        else if(toggleRotate == false && playerIsNear == false)// If toggleRotate is false and player is not near, rotate back to starting direction
        {
            // Find the vector pointing from our position to the starting position
            vectorToTarget = startingPosition.position - transform.position;
            // Calculate the angle from the point
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            // Create the rotation we need to be in to look at the target
            direction = Quaternion.AngleAxis(angle, Vector3.forward);
            // Rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * rotationSpeed);
        }
    }

    void FixedUpdate()
    {
        // If we don't have a path return until we do
        if (path == null)
        {
            return;
        }

        // If the current waypoint is greater than or equal to the number vectors in the path...
        if (currentWaypoint >= path.vectorPath.Count)
        {
            // We reached the end of path
            reachedEndOfPath = true;
            // Return, because we don't have a waypoint to follow anymore
            return;
        }
        else // If the current waypoint is NOT greater than or equal to the number vectors in the path...
        {
            // We did not the reach the end of path
            reachedEndOfPath = false;
        }

        // If the player is not near, continue moving along path
        if (playerIsNear == false)
        {
            // Get the direction in which the path is facing at each waypoint
            Vector2 pathDirection = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            // Calculate force based on the direction of waypoint
            Vector2 force = pathDirection * moveSpeed * Time.deltaTime;
            // Add the force
            rb.AddForce(force);
            // Get the distance between the position of AI and position of waypoint
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            // If the distance is less than next Waypoint...
            if (distance < nextWaypointDistance)
            {
                // Increment to next waypoint
                currentWaypoint++;
            }
        }
        
    }
}
