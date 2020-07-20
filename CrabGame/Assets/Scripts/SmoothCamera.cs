using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
	private Transform target;

	[SerializeField]
	private float zDistance = 10;

	[SerializeField]
	private float lerpSpeed = 4;

	// Start is called before the first frame update
	void Start()
    {
		target = GameObject.FindGameObjectWithTag("Player").transform;

		transform.position = target.position + Vector3.back * zDistance;
    }

	// Update is called once per frame
	void FixedUpdate()
	{
		float t = Time.fixedDeltaTime * lerpSpeed;
		transform.position = Vector3.Lerp(transform.position, target.position + Vector3.back * zDistance, t);
		transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, t);
	}
}
