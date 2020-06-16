using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMovementVariant
{
	Version1,
	Version2
}

public enum EnemyState
{
	Sleeping,
	Aggro,
	Mirroring,
	Retreating
}

public class EnemyMovement : MonoBehaviour
{
	public PlayerMovement player;
	public GameObject sleepPoint;

	public float aggroDistance = 10;
	public float stoppingDistance = 1.5f;
	public float sleepingDistance = 0.5f;

	EnemyState state = EnemyState.Sleeping;

	// Movement
	public EnemyMovementVariant movementMechanics = EnemyMovementVariant.Version1;
	public float aggroSpeed = 5;
	public float retreatSpeed = 7;

	// Mirroring
	public PlayerRotationVariant turningMechanics = PlayerRotationVariant.Version1;
	public float walkSpeed = 6;
	public float notStrafingPenalty = 0.67f;
	public float whileRotatingPenalty = 0.5f;
	public float rotateSpeed = 60; // In degrees per second

	private void Update()
	{
		float deltaTime = Time.deltaTime;


		Vector3 dif = (player.transform.position - transform.position);
		float distance = Vector3.SqrMagnitude(dif);

		Vector3 sleepDif = (sleepPoint.transform.position - transform.position);
		float sleepDistance = Vector3.SqrMagnitude(sleepDif);

		if (distance < aggroDistance * aggroDistance)
		{
			if (distance > stoppingDistance * stoppingDistance)
				state = EnemyState.Aggro;
			else
				state = EnemyState.Mirroring;
		}
		else
		{
			if (sleepDistance > sleepingDistance * sleepingDistance)
				state = EnemyState.Retreating;
			else
				state = EnemyState.Sleeping;
		}


		if (state == EnemyState.Aggro)
		{
			if (movementMechanics == EnemyMovementVariant.Version1)
				transform.Translate(dif.normalized * aggroSpeed * deltaTime, Space.Self);
			else if(movementMechanics == EnemyMovementVariant.Version2)
				transform.Translate(dif.normalized * aggroSpeed * deltaTime, Space.World);

			float angle = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg - 90;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
		else if (state == EnemyState.Mirroring)
		{
			float angle = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg - 90;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
		else if (state == EnemyState.Retreating)
		{
			transform.Translate(sleepDif.normalized * retreatSpeed * deltaTime, Space.World);

			float angle = Mathf.Atan2(sleepDif.y, sleepDif.x) * Mathf.Rad2Deg - 90;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
		else if (state == EnemyState.Sleeping)
		{

		}
	}

	// Update is called once per frame
	void MirrorInput()
	{
		float deltaTime = Time.deltaTime;

		float h = -Input.GetAxis("Horizontal");
		float v = -Input.GetAxis("Vertical");

		float epsilon = 0.1f;

		bool isRotating = false;
		if (turningMechanics == PlayerRotationVariant.Version1)
			isRotating = Mathf.Abs(h) > epsilon && v < -epsilon;
		if (turningMechanics == PlayerRotationVariant.Version2)
			isRotating = Mathf.Abs(h) > epsilon && Mathf.Abs(v) > epsilon;

		if (isRotating)
			transform.Rotate(0, 0, -1 * h * rotateSpeed * deltaTime);

		transform.Translate(new Vector3(h, (isRotating ? v * whileRotatingPenalty : v) * notStrafingPenalty, 0) * walkSpeed * deltaTime, Space.Self);
	}
}
