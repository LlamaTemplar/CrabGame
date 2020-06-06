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

    public enum EnemyAction {Attack, Block};
    public EnemyAction currentAction;

    public float cooldown;
    private float startCooldown = 4f;
    public float attackDistance = 4;
    public bool isAttacking = false;

    public float blockDistance = 4;
    public bool isBlocking = false;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        //random action, note that min is inclusive and max is exclusive, so range is from 0-1, NOT 0-2
        currentAction = (EnemyAction)Random.Range(0, 2);

        rightOGpos = rightArm.transform.localPosition;
        leftOGpos = leftArm.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //if the Player is in wake up distance and we are not blocking...
        if (Vector2.Distance(transform.position, target.position) <= wakingDistance && (isBlocking == false && isAttacking == false))
        {
            //wake up the Enemy
            isSleeping = false;
            //while the player is not within stopping distacnce....
            if (Vector2.Distance(transform.position, target.position) > stoppingDistance && (isBlocking == false && isAttacking == false))
            {
                //the enemy is aggroed to Player
                aggro = true;
                //move the enemy to Player
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else //if within stopping distance then stop aggro
            {
                aggro = false;
            }
        }
        else //if the player is not within waking distance...
        {
            //if the enmey moved away from starting position...
            if (Vector2.Distance(transform.position, startingPosition) != 0)
            {
                aggro = false;
                //move enemy back to starting position
                transform.position = Vector2.MoveTowards(transform.position, startingPosition, speed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, startingPosition) == 0)
            {
                //put enemy to sleep
                isSleeping = true;
            }
        }

        //if cooldown is not counting down...
        if (cooldown <= 0)
        {
            cooldown = 0;
            //if the enemy is not sleeping and not aggro
            if (isSleeping == false && aggro == false)
            {
                //if current action is Attack and Player is with in attack distance...
                if (currentAction == EnemyAction.Attack && Vector2.Distance(transform.position, target.position) <= attackDistance)
                {
                    //Attack Code
                    Attack();
                }
                else if (currentAction == EnemyAction.Block && Vector2.Distance(transform.position, target.position) <= blockDistance)//if action is block and Player is in Block range
                {
                    //Block Code
                    Block();
                }
            }
        }
        else//AFter action start cooldown
        {
            //Cooldown counting down
            cooldown -= Time.deltaTime;

            //move rightarm back to orignal position, note that this code will be moved when an right arm attack animation is added to better time the move arm back
            rightArm.transform.localPosition = rightOGpos;
            //move leftarm back to orignal position, note that this code will be moved when an left arm attack animation is added to better time the move arm back
            leftArm.transform.localPosition = leftOGpos;
        }
    }

    void Attack()
    {
        //the enemy is attacking
        isAttacking = true;
        //check if enemy has an arm for an attack
        if (rightArm.GetComponent<Arm>().loseArm == false || leftArm.GetComponent<Arm>().loseArm == false)
        {
            //if we have both arms, attack with both
            if (rightArm.GetComponent<Arm>().loseArm == false && leftArm.GetComponent<Arm>().loseArm == false)
            {
                //move right arm so collider can hit player
                rightArm.transform.localPosition = new Vector3(rightArm.transform.localPosition.x, rightArm.transform.localPosition.y + 0.08f, rightArm.transform.localPosition.z);
                //move left arm so collider can hit player
                leftArm.transform.localPosition = new Vector3(leftArm.transform.localPosition.x, leftArm.transform.localPosition.y + 0.08f, leftArm.transform.localPosition.z);

                //if statement for checking colliders
                //then if Player is in Collider deal damage by calling on Enemy DealDamage()
                //Attack Animation
                print("Both Arms Attacking");
            }
            else if (rightArm.GetComponent<Arm>().loseArm == true && leftArm.GetComponent<Arm>().loseArm == false)//if we lost right arm, attack with left
            {
                //move left arm so collider can hit player
                leftArm.transform.localPosition = new Vector3(leftArm.transform.localPosition.x, leftArm.transform.localPosition.y + 0.08f, leftArm.transform.localPosition.z);

                //if statement for checking colliders
                //then if Player is in Collider deal damage by calling on Enemy DealDamage()
                //Attack Animation
                print("Left Arm Attacking");
            }
            else if (rightArm.GetComponent<Arm>().loseArm == false && leftArm.GetComponent<Arm>().loseArm == true)//if we lost left arm, attack with rightt
            {
                //move right arm so collider can hit player
                rightArm.transform.localPosition = new Vector3(rightArm.transform.localPosition.x, rightArm.transform.localPosition.y + 0.08f, rightArm.transform.localPosition.z);

                //if statement for checking colliders
                //then if Player is in Collider deal damage by calling on Enemy DealDamage()
                //Attack Animation
                print("Right Arm Attacking");
            }

            //start cooldown 
            cooldown = startCooldown;
        }
        //Enemy is no longer attacking 
        isAttacking = false;
        //new random action
        currentAction = (EnemyAction)Random.Range(0, 2);
    }

    void Block()
    {
        //the enemy is blocking
        isBlocking = true;
        //check if enemy has both arms for an block
        if (rightArm.GetComponent<Arm>().loseArm == false && leftArm.GetComponent<Arm>().loseArm == false)
        {
            //Block Animation
            print("Blocking");

            //start cooldown 
            cooldown = startCooldown;
        }
        //Enemy is no longer blocking 
        isBlocking = false;
        //new random action
        currentAction = (EnemyAction)Random.Range(0, 2);
    }
}
