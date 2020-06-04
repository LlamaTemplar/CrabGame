using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    private Transform startingPosition;
    private float speed = 10;
    private Transform target;
    public bool isSleeping = false;
    public bool aggro = false;
    public float wakingDistance = 15;
    public float stoppingDistance = 6;

    public GameObject rightArm;
    public GameObject leftArm;

    public enum EnemyAction {Attack, Block};
    public EnemyAction currentAction;

    public float attackDistance = 4;
    public float cooldown;
    private float startCooldown = 2f;
    public bool isAttacking = false;

    public float blockDistance = 4;
    public bool isBlocking = false;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform;
        currentAction = (EnemyAction)Random.Range(0, 1);
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
            if (Vector2.Distance(transform.position, startingPosition.position) != 0)
            {
                //move enemy back to starting position
                transform.position = Vector2.MoveTowards(transform.position, startingPosition.position, speed * Time.deltaTime);
            }
            //put enemy to sleep
            isSleeping = true;
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
        }
    }

    void Attack()
    {
        //the enemy is attacking
        isAttacking = true;
        //check if enemy has an arm for an attack
        if (rightArm.GetComponent<Arm>().loseArm == false || leftArm.GetComponent<Arm>().loseArm == false)
        {
            //if statement for checking colliders
            //then if Player is in Collider deal damage by calling on Enemy DealDamage()
            //Attack Animation
            print("Attacking");

            //start cooldown 
            cooldown = startCooldown;
        }
        //Enemy is no longer attacking 
        isAttacking = false;
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
    }
}
