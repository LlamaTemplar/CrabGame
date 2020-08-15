using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action { Attack, Block, None };

public class EnemyActions2 : MonoBehaviour
{
    private Enemy enemy;
    public Player player;
    public Action currentAction;
    private int attacksInARow = 0;
    public int maxAttacksInARow = 3;
    public bool isAttacking = false;
    public float punchExtentsion;
    public float punchCooldown = 1f;
    public float timeToCancelAttack = 0.05f;
    private HitBox hitBox;
    private string hitBoxName = "hitBox";
    private int damage = 40;

    // For Animations
    private Animator animator;

    // Block variables
    public float actionTimer;
    public float actionCooldown = 1f;

    public float cooldownMin = 1f;
    public float cooldownMax = 1.5f;

    public float blockTimer = -1;
    public float minBlockLength = 0.5f;
    public float maxBlockLength = 1.5f;
    //public bool isBlocking = false; 

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<Player>();
        //currentAction = (Action)Random.Range(0, 2);
        currentAction = Action.Block;

        //punchExtentsion = timeToCancelAttack;
        punchExtentsion = 0;

        blockTimer = Random.Range(minBlockLength, maxBlockLength);

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            print("animator is missing");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (actionTimer <= 0)
        {
            //// New Code
            if (IsTargetInRange() && CheckCanBlock())
            {
                // shouldn't be called every frame
                Block();
            }
            else if (enemy.isBlocking && (blockTimer <= 0 || !CheckCanBlock()))
            {
                ReleaseBlock();
            }

            if (isAttacking && !enemy.isBlocking)
            {
                punchExtentsion += Time.deltaTime; // reset this...

                if (punchExtentsion > timeToCancelAttack && hitBox == null)
                {
                    CreateHitBox();
                    StartAttackAnim();
                }
            }
            // shouldn't be able to start after an attack is cancel
            else if (IsTargetInRange() && CheckCanAttack()) // start attack
            {
                isAttacking = true;
            }
        }
        else
        {
            actionTimer -= Time.deltaTime;
        }
    }

    bool IsTargetInRange()
    {
        Vector3 dif = new Vector3();
        if (player != null)
        {
            dif = (player.transform.position - transform.position);
        }
        //Vector3 dif = (player.transform.position - transform.position);
        float dist = Vector3.SqrMagnitude(dif);
        float stoppingDistance = 4.1f;

        if (dist > stoppingDistance)
        {
            return false;
        } 

        return true;
    }

    bool CheckCanAttack()
    {
        if (actionTimer > 0)
        {
            //print("attack: a");
            return false;
        }
        else if (punchExtentsion > 0)
        {
            //print("attack: b");
            return false;
        }
        else if (enemy.isBlocking == true)
        {
            //print("attack: c");
            return false;
        }
        else if (currentAction != Action.Attack)
        {
            //print("attack: d");
            return false;
        }

        return true;
    }

    void StartAttackAnim()
    {
        //gameObject.GetComponent<Unit>().PlayPunchingSound();
        int randArm = Random.Range(0,2);

        if (randArm == 0)
        {
            SetAnimations("right", true);
        }
        else
        {
            SetAnimations("left", true);
        }
    }

    void CreateHitBox()
    {
        // need to check if destoryed later..no errors on playtime!
        hitBox = new GameObject().AddComponent<HitBox>();
        hitBox.gameObject.name = hitBoxName;
        hitBox.transform.parent = this.transform;
        hitBox.transform.position = transform.position + transform.right;
        hitBox.transform.rotation = this.transform.rotation;
        hitBox.InitializeHitBox(damage, LayerMask.GetMask("Player"));
    }

    public void DestroyHitBox()
    {
        // BUG: sometimes hitbox would be null for some reason and this method gets called
        // playeractions would stop working afterwards
        if (hitBox) Destroy(hitBox.gameObject);
        isAttacking = false;
        punchExtentsion = 0;
        actionTimer = Random.Range(cooldownMin, cooldownMax);
        PickNewAction();
    }

    // Check if the player can start blocking
    bool CheckCanBlock()
    {
        if (enemy.currentStamina < 0)
        {
            //print("block: a");
            return false;
        }
        else if (blockTimer <= 0)
        {
            //print("block: b");
            return false;
        }
        else if (actionTimer > 0)
        {
            //print("block: c");
            return false;
        }
        else if (punchExtentsion > timeToCancelAttack)
        {
            //print("block: d");
            return false;
        }
        else if (currentAction != Action.Block)
        {
            //print("block: e");
            return false;
        }

        return true;
    }

    void Block()
    {
        //isBlocking = true;
        isAttacking = false;
        punchExtentsion = 0;

        // Make player bool true, to prevent taking damage
        enemy.isBlocking = true;
        // Blocking Animation
        SetAnimations("block", true);

        if (blockTimer > 0)
        {
            blockTimer -= Time.deltaTime;
        }
    }

    void ReleaseBlock()
    {
        blockTimer = Random.Range(minBlockLength, maxBlockLength);
        // Blocking cooldown begins counting down
        //actionTimer = actionCooldown;
        // Generate a cooldown between a min and max value
        actionTimer = Random.Range(cooldownMin, cooldownMax);

        //isBlocking = false;

        // Make player bool false, to prevent taking damage
        enemy.isBlocking = false;
        // Stop Blocking Animation
        SetAnimations("block", false);
        SetAnimatorSpeed(1f);

        PickNewAction();
    }

    // Pick an action based on the current context
    void PickNewAction()
    {
        if (currentAction == Action.Block)
        {
            currentAction = Action.Attack;
        }
        else
        {
            currentAction = (Action)Random.Range(0, 2);
        }

        // Don't attack too much
        if (currentAction == Action.Attack)
        {
            attacksInARow++;
            if (attacksInARow > maxAttacksInARow)
            {
                currentAction = Action.Block;
                attacksInARow = 0;
            }
        }
    }

    public void SetAnimations(string anim, bool b)
    {
        if (animator != null)
        {
            if (anim.Equals("left"))
            {
                animator.SetBool("IsAttack_LeftClaw", b);
            }
            else if (anim.Equals("right"))
            {
                animator.SetBool("IsAttack_RightClaw", b);
            }
            else if (anim.Equals("block"))
            {
                animator.SetBool("IsDefend", b);
            }
            else if (anim.Equals("IsMove"))
            {
                animator.SetBool(anim, b);
            }
        }
    }

    public void SetAnimatorSpeed(float num)
    {
        animator.SetFloat("BlockAniSpd", num);
    }
}
