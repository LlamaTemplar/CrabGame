using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePrototype2 : MonoBehaviour
{

	public float walkSpeed = 6;
	private GameObject crabBody;

	private void Start()
	{
		crabBody = transform.GetChild(0).gameObject;
	}

	// Update is called once per frame
	void Update()
	{
		float deltaTime = Time.deltaTime;

		// changed this to raw, made controls more responsive
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		// Rotate in place
		if (Input.GetKeyDown(KeyCode.Q))
			crabBody.transform.Rotate(new Vector3(0, 0, 90), Space.World);
		if(Input.GetKeyDown(KeyCode.E))
			crabBody.transform.Rotate(new Vector3(0, 0, -90), Space.World);

		// move
		transform.Translate(new Vector3(h , v, 0) * walkSpeed * deltaTime, Space.World);


	}
}
