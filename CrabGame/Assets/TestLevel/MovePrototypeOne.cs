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
	void FixedUpdate()
	{
		float deltaTime = Time.fixedDeltaTime;

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

		if (gameObject.GetComponent<Unit>().isKnockedBack)
		{
			hSmooth = 0f;
			vSmooth = 0f;
		}

		transform.Translate(new Vector3(hSmooth * notStrafingPenalty, (isRotating ? vSmooth * whileRotatingPenalty : vSmooth), 0) * walkSpeed * deltaTime, Space.Self);
	}
}
