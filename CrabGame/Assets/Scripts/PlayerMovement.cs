using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
	// Walking
	public float walkSpeed;
	public float ogWalkSpeed = 6;
	public float notStrafingPenalty = 0.3f;
	public float whileRotatingPenalty = 0.5f;

	// Rotating
	public float rotateSpeed = 100; // In degrees per second

	void Start()
	{
		walkSpeed = ogWalkSpeed;
	}

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
		isRotating = Mathf.Abs(hRaw) > epsilon && Mathf.Abs(vRaw) > epsilon;

		if (isRotating)
		{
			transform.Rotate(0, 0, -1 * hRaw * rotateSpeed * deltaTime);
		}

		if (gameObject.GetComponent<Unit>().isKnockedBack)
		{
			//hSmooth = 0f;
			//vSmooth = 0f;
			walkSpeed = 0f;
		}
		else
		{
			walkSpeed = ogWalkSpeed;
		}

		transform.Translate(new Vector3(hSmooth * notStrafingPenalty, (isRotating ? vSmooth * whileRotatingPenalty : vSmooth), 0) * walkSpeed * deltaTime, Space.Self);
	}
}
