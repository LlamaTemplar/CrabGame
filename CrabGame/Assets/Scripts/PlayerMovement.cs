using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	//Walking
	public float walkSpeed = 6;
	public float notStrafingPenalty = 0.67f;
	public float whileRotatingPenalty = 0.5f;

	//Rotating
	public float rotateSpeed = 60; //In degrees per second

	// Update is called once per frame
	void Update()
	{
		float deltaTime = Time.deltaTime;

		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		bool isRotating = Mathf.Abs(h) > Mathf.Epsilon && Mathf.Abs(v) > Mathf.Epsilon;

		if (isRotating)
			transform.Rotate(0, -1 * h * rotateSpeed * deltaTime, 0);

		transform.Translate(new Vector3(h, 0, (isRotating ? v * whileRotatingPenalty : v) * notStrafingPenalty) * walkSpeed * deltaTime, Space.Self);


	}
}
