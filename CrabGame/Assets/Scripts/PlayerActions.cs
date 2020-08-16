using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerActions : MonoBehaviour
{
    private Player player;
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
    public float actionCooldownTimer;
    public float actionCooldown = 1f;
    //public bool isBlocking = false;

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

        //punchExtentsion = timeToCancelAttack;
        punchExtentsion = 0;

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
        //// New Code
        if (HasBlockInput() && CheckCanBlock())
        {
            // shouldn't be called every frame
            Block();
        }
        else if (player.isBlocking && (HasBlockReleaseInput() || !CheckCanBlock()))
        {
            ReleaseBlock();
        }
        else if (actionCooldownTimer > 0) // on cooldown
        {
            actionCooldownTimer -= Time.deltaTime;
        }

        // assuming only one is true
        var leftAttack = HasLeftPunchInput();
        var rightAttack = HasRightPunchInput();
        var attackInput = leftAttack || rightAttack;
        var attackingArm = leftAttack ? arms[0] : arms[1];

        if (isAttacking && !player.isBlocking)
        {
            punchExtentsion += Time.deltaTime; // reset this...

            if (punchExtentsion > timeToCancelAttack && hitBox == null)
            {
                CreateHitBox();
                StartAttackAnim();
            }
        } 
        // shouldn't be able to start after an attack is cancel
        else if (attackInput && CheckCanAttack()) // start attack
        {
            this.attackingArm = attackingArm;
            isAttacking = true;
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

    bool CheckCanAttack()
    {
        if (actionCooldownTimer > 0)
        {
            return false;
        }
        else if(punchExtentsion > 0)
        {
            return false;
        }
        else if(player.isBlocking == true)
        {
            return false;
        }
        else if (hitBox != null)
        {
            return false;
        }

        return true;
    }

    void StartAttackAnim()
    {
        GetComponent<Unit>().PlayMissSound();
        if(attackingArm.side == ArmSide.Right)
        {
            SetPunchAnim("IsAttack_RightClaw", true);
        } 
        else
        {
            SetPunchAnim("IsAttack_LeftClaw", true);
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
        //hitBox.transform.Rotate(0,0,-90);
        print(damage);
        hitBox.InitializeHitBox(damage, LayerMask.GetMask("Enemy"));
    }

    public void DestroyHitBox()
    {
        // BUG: sometimes hitbox would be null for some reason and this method gets called
        // playeractions would stop working afterwards
        if(hitBox) Destroy(hitBox.gameObject);
        isAttacking = false;
        punchExtentsion = 0;
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

    // Check if the player can start blocking
    bool CheckCanBlock()
    {      
        if (player.currentStamina < 0)
        {
            return false;
        }
        else if (actionCooldownTimer > 0)
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
        //isBlocking = true;
        isAttacking = false;
        punchExtentsion = 0;

        // Make player bool true, to prevent taking damage
        player.isBlocking = true;
        // Blocking Animation
        SetAnimations("block",true);
    }

    void ReleaseBlock()
    {
        // Blocking cooldown begins counting down
        actionCooldownTimer = actionCooldown;
        //isBlocking = false;

        // Make player bool false, to prevent taking damage
        player.isBlocking = false;
        // Stop Blocking Animation
        SetAnimations("block", false);
        SetAnimatorSpeed(1f);
    }

    public void SetPunchAnim(string anim, bool b)
    {
        animator.SetBool(anim, b);
    }

    public void SetAnimations(string anim, bool b)
    {
        if (animator != null)
        {
            if (anim.Equals("block"))
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
