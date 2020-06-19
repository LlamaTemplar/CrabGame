using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveRotate : MonoBehaviour
{
    // Values that will be set in the Inspector
    public Transform target;
    public float rotationSpeed;

    // Values for internal use
    public Vector3 vectorToTarget;
    public float angle;
    public Quaternion direction;

    // Random Rotation timer
    public float rotateTimer;
    public float startRotateTimer = 10f;

    // Start is called before the first frame update
    void Start()
    {
        vectorToTarget = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // If the Player is in Range, rotate to face the Player
        if (Vector2.Distance(transform.position, target.position) <= 8)
        {
            rotateTimer = 0;

            // Find the vector pointing from our position to the target
            vectorToTarget = target.position - transform.position;
            // Calculate the angle from the point
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            // Create the rotation we need to be in to look at the target
            direction = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            // Rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * rotationSpeed);
        }
        else // If the Player is out of Range, then look in random direction
        {
            // If timer is at 0, rotate in random direction
            if (rotateTimer <= 0)
            {
                rotateTimer = 0;

                // Give Random vector pointing from our position to the random position
                Vector3 randomPosition = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
                vectorToTarget = target.position - randomPosition;
                
                // Start timer
                rotateTimer = startRotateTimer;
            }
            else // If timer is greater than 0, start counting
            {
                rotateTimer -= Time.deltaTime;
            }

            // Calculate the angle from the point
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            // Create the rotation we need to be in to look at the target
            direction = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            // Rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * rotationSpeed);
        }
    }
}
