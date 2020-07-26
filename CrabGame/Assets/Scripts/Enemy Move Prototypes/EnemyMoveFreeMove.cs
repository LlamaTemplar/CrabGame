using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMovementVariant
{
	CirclingThePlayer,
	StraightLineToPlayer
}

public enum EnemyState
{
	Sleeping,
	Aggro,
	Mirroring,
	Retreating
}

public class EnemyMoveFreeMove : MonoBehaviour
{
	// changed type from playermovement to MovePrototype1
	private MovePrototypeOne player;

	public GameObject sleepPoint;

	public float aggroRange = 10;
	public float stoppingDistance = 1.5f;
	private float sleepingDistance = 0.5f;

	EnemyState state = EnemyState.Sleeping;

	// Movement
	public EnemyMovementVariant movementMechanics = EnemyMovementVariant.StraightLineToPlayer;

	[Tooltip("If false, the enemy is always aggro")]
	public bool useAggro = true;
	[Tooltip("If true, the enemy stays aggro as long as it stays close to the player")]
	public bool chasePastAggroRange = true;
	[Tooltip("If true, the enemy cannot move")]
	public bool isStatic = false;
	[Tooltip("If true, the enemy moves at the fast speed instead")]
	public bool useFastSpeed = false;
	public float walkSpeedSlow = 5;
	public float walkSpeedFast = 10;
	private float walkSpeed = 5;
	public float retreatSpeed = 7;

	//// Mirroring
	//public PlayerRotationVariant turningMechanics = PlayerRotationVariant.Version1;
	//public float walkSpeed = 6;
	//public float notStrafingPenalty = 0.67f;
	//public float whileRotatingPenalty = 0.5f;
	//public float rotateSpeed = 60; // In degrees per second

	private void Awake()
	{
		// BUG REPORT
		// All gameobjects in the player prefab have the tag player, which one was it looking for? 
		// Apparently it would find the wrong one in the build
		// Additionally, this script was looking for the wrong script (not your fault)
		// I changed the type from PlayerMovement to MovePrototypeOne
		//
		// thanks

		//player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovePrototypeOne>();
		player = GameObject.FindObjectOfType<MovePrototypeOne>();
	}

	private void Update()
	{
		if(player != null)
		{
			float deltaTime = Time.deltaTime;

			Vector3 dif = (player.transform.position - transform.position);
			float dist = Vector3.SqrMagnitude(dif);

			Vector3 aggroDif = (player.transform.position - sleepPoint.transform.position);
			float aggroDist = Vector3.SqrMagnitude(aggroDif);

			Vector3 sleepDif = (sleepPoint.transform.position - transform.position);
			float sleepDist = Vector3.SqrMagnitude(sleepDif);

			if (useAggro)
			{
				if ((chasePastAggroRange ? dist : aggroDist) < aggroRange * aggroRange)
				{
					if (dist > stoppingDistance * stoppingDistance)
						state = EnemyState.Aggro;
					else
						state = EnemyState.Mirroring;
				}
				else
				{
					if (sleepDist > sleepingDistance * sleepingDistance)
						state = EnemyState.Retreating;
					else
						state = EnemyState.Sleeping;
				}
			}
			else
				state = EnemyState.Aggro;

			if (gameObject.GetComponent<Unit>().isKnockedBack)
			{
				walkSpeed = 0;
			}
			else
			{
				walkSpeed = useFastSpeed ? walkSpeedFast : walkSpeedSlow;
			}
			

			if (state == EnemyState.Aggro)
			{
				if (!isStatic)
				{
					if (movementMechanics == EnemyMovementVariant.CirclingThePlayer)
						transform.Translate(dif.normalized * walkSpeed * deltaTime, Space.Self);
					else if (movementMechanics == EnemyMovementVariant.StraightLineToPlayer)
						transform.Translate(dif.normalized * walkSpeed * deltaTime, Space.World);
				}

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
				if (!isStatic)
				{
					transform.Translate(sleepDif.normalized * retreatSpeed * deltaTime, Space.World);

					float angle = Mathf.Atan2(sleepDif.y, sleepDif.x) * Mathf.Rad2Deg - 90;
					transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				}
			}
			else if (state == EnemyState.Sleeping)
			{

			}
		}
		else
		{
			// This is causing builds to not work
			//Debug.LogError("Missing Reference to player");
		}
	}

	//// Update is called once per frame
	//void MirrorInput()
	//{
	//	float deltaTime = Time.deltaTime;

	//	float h = -Input.GetAxis("Horizontal");
	//	float v = -Input.GetAxis("Vertical");

	//	float epsilon = 0.1f;

	//	bool isRotating = false;
	//	if (turningMechanics == PlayerRotationVariant.Version1)
	//		isRotating = Mathf.Abs(h) > epsilon && v < -epsilon;
	//	if (turningMechanics == PlayerRotationVariant.Version2)
	//		isRotating = Mathf.Abs(h) > epsilon && Mathf.Abs(v) > epsilon;

	//	if (isRotating)
	//		transform.Rotate(0, 0, -1 * h * rotateSpeed * deltaTime);

	//	transform.Translate(new Vector3(h, (isRotating ? v * whileRotatingPenalty : v) * notStrafingPenalty, 0) * walkSpeed * deltaTime, Space.Self);
	//}
}
