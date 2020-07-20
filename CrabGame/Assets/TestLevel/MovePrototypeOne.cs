using UnityEngine;


public class MovePrototypeOne : MonoBehaviour
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
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		float epsilon = 0.1f;

		bool isRotating = false;
		//if (turningMechanics == PlayerRotationVariant.Version1)
		isRotating = Mathf.Abs(h) > epsilon && Mathf.Abs(v) > epsilon;
		//if (turningMechanics == PlayerRotationVariant.Version2)
		//	isRotating = Mathf.Abs(h) > epsilon && Mathf.Abs(v) > epsilon;

		if (isRotating)
		{
			if (turningMechanics == PlayerRotationVariant.Version1)
				transform.Rotate(0, 0, -1 * h * rotateSpeed * deltaTime);
			if (turningMechanics == PlayerRotationVariant.Version2)
				transform.Rotate(0, 0, Mathf.Sign(v) * h * rotateSpeed * deltaTime);
		}

		if (gameObject.GetComponent<Unit>().isKnockedBack)
		{
			h = 0f;
			v = 0f;
		}
		transform.Translate(new Vector3(h * notStrafingPenalty, (isRotating ? v * whileRotatingPenalty : v) , 0) * walkSpeed * deltaTime, Space.Self);
	}
}
