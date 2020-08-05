using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
	private Transform target;

	[SerializeField]
	private float zDistance = 10;

	[SerializeField]
	private float blendRatePosition = 4;

	[SerializeField]
	private float blendRateRotation = 5;

	// Start is called before the first frame update
	void Start()
    {
		target = GameObject.FindGameObjectWithTag("Player").transform;

		// The camera always has to be a certain distance away in order to see anything
		transform.position = target.position + Vector3.back * zDistance;
    }

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!target)
			return;

		// !!! If the target moves on FixedUpdate(), use FixedUpdate() here !!!
		float deltaTime = Time.fixedDeltaTime;

		// Smoothly match the target's transform
		transform.position = Vector3.Lerp(transform.position, target.position + Vector3.back * zDistance, deltaTime * blendRatePosition);
		transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, deltaTime * blendRateRotation);
	}
}
