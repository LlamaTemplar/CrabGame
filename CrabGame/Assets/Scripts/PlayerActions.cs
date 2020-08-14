using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerActions : MonoBehaviour
{

    private Player player;
    public bool isAttacking = false;
    public float postAttackCoolDown;
    public float punchExtentsion;
    public float timeToCancelAttack = 0.2f;
    private HitBox hitBox;
    private string hitBoxName = "HitBox";
    private int damage = 40;

    // For Both Arms
    private Animator animator;

    // Right Arm variables
    public GameObject rightArm;
    public float rightCooldown;
    public float rightStartTime = 1f;

    // Left Arm variables 
    public GameObject leftArm;
    public float leftCooldown;
    public float leftStartTime = 1f;

    // Block variables
    public float blockCooldown;
    public float startBlockTime = 1.5f;
    public bool isBlocking = false;
    // Will most likely be removed these 3 lines after adding animations

    struct PlayerArm
    {
        public ArmSide side;
    }

    PlayerArm[] arms = new PlayerArm[2];
    enum ArmSide {Left, Right};
    private PlayerArm attackingArm;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();

        punchExtentsion = timeToCancelAttack;


        arms[0].side = ArmSide.Left;

        arms[1].side = ArmSide.Right;

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            print("animator is missing");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //// New Code Uwu

        if (HasBlockInput() && CheckCanBlock())
        {
            // shouldn't be called every frame
            Block();
        }
        else if (isBlocking && (HasBlockReleaseInput() || !CheckCanBlock()))
        {
            ReleaseBlock();
        }
        else if (blockCooldown > 0) // on cooldown
        {
            blockCooldown -= Time.deltaTime;
        }

        // assuming only one is true
        var leftAttack = HasLeftPunchInput();
        var rightAttack = HasRightPunchInput();
        var attackInput = leftAttack || rightAttack;
        var attackingArm = leftAttack ? arms[0] : arms[1];
        
        if (isAttacking && !isBlocking)
        {
            punchExtentsion += Time.deltaTime; // reset this...

            if (punchExtentsion > timeToCancelAttack && hitBox == null) 
                CreateHitBox();
        } 
        // shouldn't be able to start after an attack is cancel
        else if (attackInput && CheckCanAttack(attackingArm)) // start attack
        {
            this.attackingArm = attackingArm;
            isAttacking = true;
            StartAttack();
        }
        else // on cooldown
        {
            postAttackCoolDown -= Time.deltaTime;
        }
    }

    bool HasLeftPunchInput()
    {
        return Input.GetKeyDown(KeyCode.J);
    }

    bool HasRightPunchInput()
    {
        return Input.GetKeyDown(KeyCode.K);
    }

    bool CheckCanAttack(PlayerArm arm)
    {
        if(postAttackCoolDown > 0)
        {
            return false;
        }
        else if(punchExtentsion != timeToCancelAttack)
        {
            return false;
        }
        else if(isBlocking == true)
        {
            return false;
        }

        return true;
    }

    void StartAttack()
    {
        gameObject.GetComponent<Unit>().PlayPunchingSound();

        if(attackingArm.side == ArmSide.Right)
        {
            SetAnimations("right", true);
        } else
        {
            SetAnimations("left", true);
        }
    }

    void CreateHitBox()
    {
        // need to check if destoryed later..no errors on playtime!
        hitBox = new GameObject().AddComponent<HitBox>();
        hitBox.gameObject.name = "hitBox";
        hitBox.transform.parent = this.transform;
        hitBox.transform.position = transform.position + transform.right;
        hitBox.transform.rotation = this.transform.rotation;
        hitBox.InitializeHitBox(damage, LayerMask.GetMask("Enemy"));
    }

    public void DestoryHitBlock()
    {
        // BUG: sometimes hitbox would be null for some reason and this method gets called
        // playeractions would stop working afterwards
        if(hitBox) Destroy(hitBox.gameObject);
        isAttacking = false;
        punchExtentsion = timeToCancelAttack;
    }

    bool HasBlockInput()
    {
        
        // If both arm keys are pressed and the both their delays are greater than 0
        return Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.J);
    }

    bool HasBlockReleaseInput()
    {
        // While blocking, when keys are released undo blocking
        return Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.J);
    }

    void ReleaseBlock()
    {
        // Blocking cooldown begins counting down
        blockCooldown = startBlockTime;
        isBlocking = false;
        var b = false;

        // Make player bool false, to prevent taking damage
        gameObject.GetComponent<Player>().isBlocking = b;
        // Stop Blocking Animation
        SetAnimations("block", false);
        SetAnimatorSpeed(1f);
    }

    // Check if the player can start blocking
    bool CheckCanBlock()
    {      
        if (player.currentStamina < 0)
        {

            return false;
        }
        else if (blockCooldown > 0)
        {

            return false;
        }
        else if (punchExtentsion > timeToCancelAttack) 
        {
            return false;
        } 


        return true;
    }

    void Block()
    {
        isBlocking = true;
        var b = true;

        // Make player bool true, to prevent taking damage
        gameObject.GetComponent<Player>().isBlocking = b;
        // Blocking Animation
        SetAnimations("block",true);
    }

    void PlaySounds(GameObject arm)
    {
        if (arm.GetComponent<Arm>().GetTheEnemy() != null)
        {
            gameObject.GetComponent<Unit>().PlayPunchingSound();
        }
        else
        {
            gameObject.GetComponent<Player>().PlayMissSound();
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
            else if(anim.Equals("right"))
            {
                animator.SetBool("IsAttack_RightClaw", b);
            }
            else if (anim.Equals("block"))
            {
                animator.SetBool("IsDefend",b);
            }
            else if (anim.Equals("IsMove"))
            {
                animator.SetBool(anim, b);
            }
        }
    }

    public void SetAnimatorSpeed(float num)
    {
        animator.SetFloat("BlockAniSpd",num);
    }
}
