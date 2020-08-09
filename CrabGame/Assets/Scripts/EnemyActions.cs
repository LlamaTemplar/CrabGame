using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAction { Attack, Block, None };

public class EnemyActions : MonoBehaviour
{
	private float incrementByNum = 0.02f;
	private float currentIncrement = 0f;
	private float incrementTotal = 0.08f;

	public Transform hitArea;
	public LayerMask whatIsPlayer;
	public bool canAct = false;

	public GameObject rightArm;
	public GameObject leftArm;
	private Vector3 rightOGpos;
	private Vector3 leftOGpos;

	public EnemyAction currentAction;

	// Note that this is a shared cooldown for both blocking and attacking
	public float actionTimer = -1f;
	public float cooldownMin = 1f;
	public float cooldownMax = 1.5f;
	private float lastCooldownPicked;

	public float blockTimer = -1;
	public float minBlockLength = 0.5f;
	public float maxBlockLength = 1.5f;
	// Will most likely be removed these 3 lines after adding animations
	private Vector3 rightBlockPos;
	private Vector3 leftBlockPos;
	public GameObject subBlockSprite;

	private bool once = false;
	public int attacksInARow = 0;
	public int maxAttacksInARow = 2;

	// Start is called before the first frame update
	void Start()
	{
		// REMOVED Follow Player Code
		// Store starting position
		/*startingPosition = transform.position;*/

		// Random action, note that min is inclusive and max is exclusive, so range is from 0-1, NOT 0-2
		// Always start attacking
		//PickNewAction();

		// Store original Local Positions (Might also be removed after adding animation)
		rightOGpos = rightArm.transform.localPosition;
		leftOGpos = leftArm.transform.localPosition;

		// Store Block positions
		rightBlockPos = new Vector3(rightArm.transform.localPosition.x - 0.05f, rightArm.transform.localPosition.y, rightArm.transform.localPosition.z);
		leftBlockPos = new Vector3(leftArm.transform.localPosition.x + 0.05f, leftArm.transform.localPosition.y, leftArm.transform.localPosition.z);
	}

	// Update is called once per frame
	void Update()
	{
		// If cooldown is not counting down...
		if (actionTimer <= 0)
		{
			if (canAct)
			{
				if (currentAction == EnemyAction.Attack)
				{
					//Attack Code
					Attack();
				}
				else if (currentAction == EnemyAction.Block)
				{
					// Block Code
					Block();
				}
			}
		}
		else// After action start cooldown
		{
			// Cooldown counting down
			if (currentAction != EnemyAction.Block || blockTimer <= 0)
				actionTimer -= Time.deltaTime;

			// Move arms back to orignal position
			// Note that the arm is visible to help visualize, when animation is added, turn off sprite
			if (actionTimer < lastCooldownPicked - 0.5f)
			{
				rightArm.transform.localPosition = rightOGpos;
				leftArm.transform.localPosition = leftOGpos;
			}
		}

		// Blocktimer begins when its greater than 0
		if (currentAction == EnemyAction.Block)
		{
			if (blockTimer > 0)
			{
				blockTimer -= Time.deltaTime;
			}
			else if (blockTimer < 0)// Unblock when timer is 0
			{
				blockTimer = 0;
				Unblock();
			}
		}
	}

	void Attack()
	{
		// Deal damge if player is in hitbox
		rightArm.GetComponent<Arm>().SetCollider(true);
		leftArm.GetComponent<Arm>().SetCollider(true);
		rightArm.GetComponent<Arm>().SetAttackingTrue();
		leftArm.GetComponent<Arm>().SetAttackingTrue();

		// Move arm smoothly
		if (currentIncrement < incrementTotal)
		{
			currentIncrement += incrementByNum;
			rightArm.transform.localPosition = new Vector3(rightArm.transform.localPosition.x, rightArm.transform.localPosition.y + incrementByNum, rightArm.transform.localPosition.z);
			leftArm.transform.localPosition = new Vector3(leftArm.transform.localPosition.x, leftArm.transform.localPosition.y + incrementByNum, leftArm.transform.localPosition.z);
		}
		else
		{
			ResetAttack();
		}
	}

	void ResetAttack()
	{
		rightArm.GetComponent<Arm>().SetCollider(false);
		leftArm.GetComponent<Arm>().SetCollider(false);
		rightArm.GetComponent<Arm>().SetAttackingFalse();
		leftArm.GetComponent<Arm>().SetAttackingFalse();

		gameObject.GetComponent<Unit>().PlayPunchingSound();
		currentIncrement = 0f;

		// Start cooldown 
		PickCooldown();

		// New random action
		PickAction();
	}

	// Pick an action based on the current context
	void PickAction()
	{
		if (currentAction == EnemyAction.Block)
		{
			currentAction = EnemyAction.Attack;
		}
		else
		{
			currentAction = (EnemyAction)Random.Range(0, 2);
		}

		// Don't attack too much
		if (currentAction == EnemyAction.Attack)
		{
			attacksInARow++;
			if (attacksInARow > maxAttacksInARow)
				currentAction = EnemyAction.Block;
		}
		else
			attacksInARow = 0;
	}

	// Generate a cooldown between a min and max value
	void PickCooldown()
	{
		actionTimer = Random.Range(cooldownMin, cooldownMax);
		lastCooldownPicked = actionTimer;
	}

	void Block()
	{
		if (once == false)
		{
			blockTimer = Random.Range(minBlockLength, maxBlockLength);
			once = true;
		}

		gameObject.GetComponent<Enemy>().isBlocking = true;
		rightArm.GetComponent<Arm>().SetCollider(true);
		leftArm.GetComponent<Arm>().SetCollider(true);

		subBlockSprite.SetActive(true);
		rightArm.transform.localPosition = rightBlockPos;
		leftArm.transform.localPosition = leftBlockPos;

		// Block Animation
	}

	void Unblock()
	{
		once = false;
		// Undo blocking code
		gameObject.GetComponent<Enemy>().isBlocking = false;
		rightArm.GetComponent<Arm>().SetCollider(false);
		leftArm.GetComponent<Arm>().SetCollider(false);

		subBlockSprite.SetActive(false);
		rightArm.transform.localPosition = rightOGpos;
		leftArm.transform.localPosition = leftOGpos;

		// Start cooldown 
		PickCooldown();
		// New random action
		PickAction();
	}

	public bool IsBlocking()
	{
		return currentAction == EnemyAction.Block;
	}
}
