using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAction { Attack, Block, None };

public class EnemyActions : MonoBehaviour
{
    // REMOVED Follow Player Code
    /*private Vector3 startingPosition;
    public Transform target;
    private float speed = 2;
    public bool isSleeping = false;
    public bool aggro = false;
    public float wakingDistance = 8;
    public float stoppingDistance = 3;*/

    public Transform hitArea;
    public LayerMask whatIsPlayer;
    public bool canAct = false;

    public GameObject rightArm;
    public GameObject leftArm;
    private Vector3 rightOGpos;
    private Vector3 leftOGpos;

    public EnemyAction currentAction;

    // Note that this is a shared cooldown for both blocking and attacking
    public float cooldown;
    private float startCooldown = 3f;

    public float attackDistance = 4;
    public bool isAttacking = false;

    public float blockDistance = 4;
    public bool isBlocking = false;
    public float blockTimer = -1;
    private float lengthOfBlock = 2.5f;
    // Will most likely be removed these 3 lines after adding animations
    private Vector3 rightBlockPos;
    private Vector3 leftBlockPos;
    public GameObject subBlockSprite;

    // Start is called before the first frame update
    void Start()
    {
        // REMOVED Follow Player Code
        // Store starting position
        /*startingPosition = transform.position;*/

        // Random action, note that min is inclusive and max is exclusive, so range is from 0-1, NOT 0-2
        currentAction = (EnemyAction)Random.Range(0, 2);

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
        // If action is block, but if we don't have both arms to block, then just attack
        if (currentAction == EnemyAction.Block && (rightArm.GetComponent<Arm>().loseArm == true || leftArm.GetComponent<Arm>().loseArm == true))
        {
            currentAction = EnemyAction.Attack;
        }
        else if (rightArm.GetComponent<Arm>().loseArm == true && leftArm.GetComponent<Arm>().loseArm == true)// If both arms are lost then crab can't do anything
        {
            currentAction = EnemyAction.None;
        }

        // REMOVED Follow Player Code
        /*// If the Player is in wake up distance and we are not blocking...
        if (Vector2.Distance(transform.position, target.position) <= wakingDistance && (isBlocking == false && isAttacking == false))
        {
            // Wake up the Enemy
            isSleeping = false;
            // While the player is not within stopping distacnce....
            if (Vector2.Distance(transform.position, target.position) > stoppingDistance && (isBlocking == false && isAttacking == false))
            {
                // The enemy is aggroed to Player
                aggro = true;
                // Move the enemy to Player
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else // If within stopping distance then stop aggro
            {
                aggro = false;
            }
        }
        else // If the player is not within waking distance...
        {
            // If the enmey moved away from starting position...
            if (Vector2.Distance(transform.position, startingPosition) != 0)
            {
                aggro = false;
                // Move enemy back to starting position
                transform.position = Vector2.MoveTowards(transform.position, startingPosition, speed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, startingPosition) == 0)
            {
                // Put enemy to sleep
                isSleeping = true;
            }
        }*/

        // If cooldown is not counting down...
        if (cooldown <= 0)
        {
            cooldown = 0;
            // If the Player is Range...
            if (canAct == true)
            {
                // If current action is Attack
                if (currentAction == EnemyAction.Attack)
                {
                    //Attack Code
                    Attack();
                }
                else if (currentAction == EnemyAction.Block)// If action is block
                {
                    // Block Code
                    Block();
                }
            }
        }
        else// After action start cooldown
        {
            // Cooldown counting down
            cooldown -= Time.deltaTime;

            // Move arms back to orignal position
            // Note that the arm is visible to help visualize, when animation is added, turn off sprite
            if (cooldown < startCooldown - 0.5f)
            {
                rightArm.transform.localPosition = rightOGpos;
                leftArm.transform.localPosition = leftOGpos;
            }
        }

        // Blocktimer begins when its greater than 0
        if (blockTimer > 0 && isBlocking == true)
        {
            blockTimer -= Time.deltaTime;
            if (rightArm.GetComponent<Arm>().loseArm == true || leftArm.GetComponent<Arm>().loseArm == true)
            {
                blockTimer = 0;
                isBlocking = false;
            }
        }
        else if(blockTimer <= 0 && isBlocking == true)// Unblock when timer is 0
        {
            blockTimer = 0;
            Unblock();
        }
    }

    void Attack()
    {
        // The enemy is attacking
        isAttacking = true;
        // Check if enemy has an arm for an attack
        if (rightArm.GetComponent<Arm>().loseArm == false || leftArm.GetComponent<Arm>().loseArm == false)
        {
            // If we have both arms, attack with both
            if (rightArm.GetComponent<Arm>().loseArm == false && leftArm.GetComponent<Arm>().loseArm == false)
            {
                // Attack Animation
                //print("Both Arms Attacking");
                rightArm.transform.localPosition = new Vector3(rightArm.transform.localPosition.x, rightArm.transform.localPosition.y + 0.08f, rightArm.transform.localPosition.z);
                leftArm.transform.localPosition = new Vector3(leftArm.transform.localPosition.x, leftArm.transform.localPosition.y + 0.08f, leftArm.transform.localPosition.z);

                // Deal damge if player is in hitbox
                rightArm.GetComponent<Arm>().SetAttacking();
                leftArm.GetComponent<Arm>().SetAttacking();
            }
            else if (rightArm.GetComponent<Arm>().loseArm == true && leftArm.GetComponent<Arm>().loseArm == false)// If we lost right arm, attack with left
            {
                //Attack Animation
               // print("Left Arm Attacking");
                leftArm.transform.localPosition = new Vector3(leftArm.transform.localPosition.x, leftArm.transform.localPosition.y + 0.08f, leftArm.transform.localPosition.z);

                //deal damge if player is in left arm hitbox
                leftArm.GetComponent<Arm>().SetAttacking();
            }
            else if (rightArm.GetComponent<Arm>().loseArm == false && leftArm.GetComponent<Arm>().loseArm == true)// If we lost left arm, attack with rightt
            {
                // Attack Animation
                //print("Right Arm Attacking");
                rightArm.transform.localPosition = new Vector3(rightArm.transform.localPosition.x, rightArm.transform.localPosition.y + 0.08f, rightArm.transform.localPosition.z);

                // Deal damge if player is in right arm hitbox
                rightArm.GetComponent<Arm>().SetAttacking();
            }

            // Start cooldown 
            cooldown = startCooldown;
        }

        // Enemy is no longer attacking 
        isAttacking = false;
        // New random action
        currentAction = (EnemyAction)Random.Range(0, 2);
        // Increase Chance of Attacking
        if (currentAction == EnemyAction.Block)
        {
            currentAction = (EnemyAction)Random.Range(0, 2);
        }
    }

    void Block()
    {
        // Check if enemy has both arms for an block
        if (rightArm.GetComponent<Arm>().loseArm == false && leftArm.GetComponent<Arm>().loseArm == false)
        {
            gameObject.GetComponent<Enemy>().isBlocking = true;
            // This if statement should only be used once instead of looping
            if (isBlocking == false)
            {
                blockTimer = lengthOfBlock;
            }

            // The enemy is blocking
            isBlocking = true;
            subBlockSprite.SetActive(true);
            rightArm.transform.localPosition = rightBlockPos;
            leftArm.transform.localPosition = leftBlockPos;

            // Block Animation
            //print("Blocking");
        }
        
    }

    void Unblock()
    {
        // Undo blocking code
        gameObject.GetComponent<Enemy>().isBlocking = false;
       // print("not blocking");

        // Enemy is no longer blocking 
        isBlocking = false;
        subBlockSprite.SetActive(false);
        rightArm.transform.localPosition = rightOGpos;
        leftArm.transform.localPosition = leftOGpos;

        // Start cooldown 
        cooldown = startCooldown;
        // Change ACtion to Attack, We don't want it to block immdiately again
        currentAction = EnemyAction.Attack;
    }
}
