using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerRotationVariant
{
	Version1,
	Version2
}

public class PlayerMovement : MonoBehaviour
{
	public PlayerRotationVariant turningMechanics = PlayerRotationVariant.Version1;

	// Walking
	public float walkSpeed = 6;
	public float notStrafingPenalty = 0.67f;
	public float whileRotatingPenalty = 0.5f;

	// Rotating
	public float rotateSpeed = 60; // In degrees per second

	// Update is called once per frame
	void Update()
	{
		float deltaTime = Time.deltaTime;

		// changed this to raw, made controls more responsive
		float hRaw = Input.GetAxisRaw("Horizontal");
		float vRaw = Input.GetAxisRaw("Vertical");

		float hSmooth = Input.GetAxis("Horizontal");
		float vSmooth = Input.GetAxis("Vertical");

		float epsilon = 0.1f;

		bool isRotating = false;
		//if (turningMechanics == PlayerRotationVariant.Version1)
			isRotating = Mathf.Abs(hRaw) > epsilon && Mathf.Abs(vRaw) > epsilon;
		//if (turningMechanics == PlayerRotationVariant.Version2)
		//	isRotating = Mathf.Abs(h) > epsilon && Mathf.Abs(v) > epsilon;

		if (isRotating)
		{
			if (turningMechanics == PlayerRotationVariant.Version1)
				transform.Rotate(0, 0, -1 * hRaw * rotateSpeed * deltaTime);
			if (turningMechanics == PlayerRotationVariant.Version2)
				transform.Rotate(0, 0, Mathf.Sign(vRaw) * hRaw * rotateSpeed * deltaTime);
		}

		transform.Translate(new Vector3(hSmooth, (isRotating ? vSmooth * whileRotatingPenalty : vSmooth) * notStrafingPenalty, 0) * walkSpeed * deltaTime, Space.Self);
	}
}
