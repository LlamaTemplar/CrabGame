using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    private Vector3 startingPosition;
    private float speed = 2;
    public Transform target;
    public bool isSleeping = false;
    public bool aggro = false;
    public float wakingDistance = 8;
    public float stoppingDistance = 3;

    public GameObject rightArm;
    public GameObject leftArm;
    private Vector3 rightOGpos;
    private Vector3 leftOGpos;

    public enum EnemyAction {Attack, Block, None};
    public EnemyAction currentAction;

    // Temp damage
    private int damage = 5;

    // Note that this is a shared cooldown for both blocking and attacking
    public float cooldown;
    private float startCooldown = 4f;

    public float attackDistance = 4;
    public bool isAttacking = false;

    public float blockDistance = 4;
    public bool isBlocking = false;
    public float blockTimer = -1;
    private float lengthOfBlock = 3f;

    // Start is called before the first frame update
    void Start()
    {
        // Store starting position
        startingPosition = transform.position;
        // Random action, note that min is inclusive and max is exclusive, so range is from 0-1, NOT 0-2
        currentAction = (EnemyAction)Random.Range(0, 2);

        // Store original Local Positions (Might also be removed after adding animation)
        rightOGpos = rightArm.transform.localPosition;
        leftOGpos = leftArm.transform.localPosition;
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

        // If the Player is in wake up distance and we are not blocking...
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
        }

        // If cooldown is not counting down...
        if (cooldown <= 0)
        {
            cooldown = 0;
            // If the enemy is not sleeping and not aggro
            if (isSleeping == false && aggro == false)
            {
                // If current action is Attack and Player is with in attack distance...
                if (currentAction == EnemyAction.Attack && Vector2.Distance(transform.position, target.position) <= attackDistance)
                {
                    //Attack Code
                    Attack();
                }
                else if (currentAction == EnemyAction.Block && Vector2.Distance(transform.position, target.position) <= blockDistance)// If action is block and Player is in Block range
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

            // Move arms back to orignal position, note that this code will be moved when an attack animation is added to better time the move arm back
            rightArm.transform.localPosition = rightOGpos;
            leftArm.transform.localPosition = leftOGpos;
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
                print("Both Arms Attacking");
                rightArm.transform.localPosition = new Vector3(rightArm.transform.localPosition.x, rightArm.transform.localPosition.y + 0.08f, rightArm.transform.localPosition.z);
                leftArm.transform.localPosition = new Vector3(leftArm.transform.localPosition.x, leftArm.transform.localPosition.y + 0.08f, leftArm.transform.localPosition.z);

                // Deal damge if player is in hitbox
                rightArm.GetComponent<Arm>().DealDamage(damage);
                leftArm.GetComponent<Arm>().DealDamage(damage);
            }
            else if (rightArm.GetComponent<Arm>().loseArm == true && leftArm.GetComponent<Arm>().loseArm == false)// If we lost right arm, attack with left
            {
                //Attack Animation
                print("Left Arm Attacking");
                leftArm.transform.localPosition = new Vector3(leftArm.transform.localPosition.x, leftArm.transform.localPosition.y + 0.08f, leftArm.transform.localPosition.z);

                //deal damge if player is in left arm hitbox
                leftArm.GetComponent<Arm>().DealDamage(damage);
            }
            else if (rightArm.GetComponent<Arm>().loseArm == false && leftArm.GetComponent<Arm>().loseArm == true)// If we lost left arm, attack with rightt
            {
                // Attack Animation
                print("Right Arm Attacking");
                rightArm.transform.localPosition = new Vector3(rightArm.transform.localPosition.x, rightArm.transform.localPosition.y + 0.08f, rightArm.transform.localPosition.z);

                // Deal damge if player is in right arm hitbox
                rightArm.GetComponent<Arm>().DealDamage(damage);
            }

            // Start cooldown 
            cooldown = startCooldown;
        }
        // Enemy is no longer attacking 
        isAttacking = false;
        // New random action
        currentAction = (EnemyAction)Random.Range(0, 2);
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

            // Block Animation
            print("Blocking");
        }
        
    }

    void Unblock()
    {
        // Undo blocking code
        gameObject.GetComponent<Enemy>().isBlocking = false;
        print("not blocking");

        // Enemy is no longer blocking 
        isBlocking = false;

        // Start cooldown 
        cooldown = startCooldown;
        // new random action
        currentAction = (EnemyAction)Random.Range(0, 2);
    }
}
