using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    // For Both Arms
    private float incrementByNum = 0.2f;
    private float currentIncrement = 0f;
    private float incrementTotal = 2f;
    private Animator animator;
    public Animation animation;

    // Right Arm variables
    public GameObject rightArm;
    public float rightCooldown;
    public float rightStartTime = 1f;
    private float rightDelay;
    private float rightStartDelay = 0.2f;
    private bool rightWindUp = false;
    private Vector3 rightOGpos;

    // Left Arm variables 
    public GameObject leftArm;
    public float leftCooldown;
    public float leftStartTime = 1f;
    private float leftDelay;
    private float leftStartDelay = 0.2f;
    private bool leftWindUp = false;
    private Vector3 leftOGpos;

    // Block variables
    public float blockCooldown;
    public float startBlockTime = 1.5f;
    public bool isBlocking = false;
    // Will most likely be removed these 3 lines after adding animations
    private Vector3 rightBlockPos;
    private Vector3 leftBlockPos; 
    public GameObject subBlockSprite;

    struct PlayerArm
    {
        public GameObject arm;
        public float coolDown;
        public float windUp;
        public float delay;
        public Vector3 startingPos;
    }

    PlayerArm[] arms = new PlayerArm[2];

    // Start is called before the first frame update
    void Start()
    {
        // left arm
        arms[0].delay = leftStartDelay;
        arms[0].startingPos = leftArm.transform.localPosition;
        // right arm
        arms[1].delay = rightStartDelay;
        arms[1].startingPos = rightArm.transform.localPosition;

        // Delays for the attacks
        //rightDelay = rightStartDelay;
        //leftDelay = leftStartDelay;

        // Store original Local Positions (Might also be removed after adding animation)
        rightOGpos = rightArm.transform.localPosition;
        leftOGpos = leftArm.transform.localPosition;

        // Store Block positions
        rightBlockPos = new Vector3(rightArm.transform.localPosition.x - 0.05f, rightArm.transform.localPosition.y, rightArm.transform.localPosition.z);
        leftBlockPos = new Vector3(leftArm.transform.localPosition.x + 0.05f, leftArm.transform.localPosition.y, leftArm.transform.localPosition.z);

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

        // Checks if the right arm button is pressed and if the cooldowntime is at 0
        if (rightCooldown <= 0)
        {
            rightCooldown = 0;
            // Check if we still have right arm
            if (rightArm.GetComponent<Arm>().lostArm == false)
            {
                // When key is pressed and the delay is at starting number and we are NOT blocking and the left arm is NOT winding up for attack
                if (Input.GetKeyDown(KeyCode.K) && rightDelay == rightStartDelay && isBlocking == false && leftWindUp == false)
                {
                    // If block is NOT on cooldown...
                    if (blockCooldown == 0)
                    {
                        // The wind up is true
                        rightWindUp = true;
                    }
                }

                // While wind up is true (i don't like while loops)
                if (rightWindUp == true)
                {
                    //delay will count down
                    rightDelay -= Time.deltaTime;
                }
            }
            else // If arm is lost or becomes lost during delay, reset delay and windup
            {
                rightDelay = rightStartDelay;
                rightWindUp = false;
            }

            // If delay counts down to 0 and Player "is not" blocking, commence attack
            if (rightDelay <= 0 && isBlocking == false)
            {
                rightDelay = 0;
                // Reset windup
                rightWindUp = false;

                // Attack code for right arm
                RightAttack();
            }
            else if (rightDelay <= 0 && isBlocking == true) // If delay counts down to 0 and Player "is" blocking, just reset the delay and make windUp false
            {
                rightDelay = rightStartDelay;
                rightWindUp = false;
            }
        }
        else // If cooldown is not 0 then begin countdown (can't attack unless cooldown is at 0)
        {
            // Cooldown counting down
            rightCooldown -= Time.deltaTime;

            // Move rightarm back to orignal position after 0.5f secs
            // Note that the arm is visible to help visualize, when animation is added, turn off sprite
            if (rightCooldown < rightStartTime - 0.5f && isBlocking == false)
            {
                rightArm.transform.localPosition = rightOGpos;
            }
        }

        // Checks if the left arm button is pressed and if the cooldowntime is at 0
        if (leftCooldown <= 0)
        {
            leftCooldown = 0;
            // Check if we still have left arm
            if (leftArm.GetComponent<Arm>().lostArm == false)
            {
                // When key is pressed and the delay is at starting number and we are NOT blocking and the right arm is NOT winding up for attack
                if (Input.GetKeyDown(KeyCode.J) && leftDelay == leftStartDelay && isBlocking == false && rightWindUp == false)
                {
                    // If block is NOT on cooldown...
                    if (blockCooldown == 0)
                    {
                        // The wind up is true
                        leftWindUp = true;
                    }
                }

                // While wind up is true (i don't like while loops)
                if (leftWindUp == true)
                {
                    // Delay will count down
                    leftDelay -= Time.deltaTime;
                }
            }
            else // If arm is lost or becomes lost during delay, reset delay and windup
            {
                leftDelay = leftStartDelay;
                leftWindUp = false;
            }

            // If delay counts down to 0 and Player "is not" blocking, commence attack
            if (leftDelay <=0 && isBlocking == false)
            {
                leftDelay = 0;
                // Reset windup
                leftWindUp = false;

                // Attack code for left arm
                LeftAttack();
            }
            else if (leftDelay <= 0 && isBlocking == true)// If delay counts down to 0 and Player "is" blocking, just reset the delay and make windUp false
            {
                leftDelay = leftStartDelay;
                leftWindUp = false;
            }
        }
        else // If cooldown is not 0 then begin countdown (can't attack unless cooldown is at 0)
        {
            // Cooldown counting down
            leftCooldown -= Time.deltaTime;

            // Move Leftarm back to orignal position after 0.5f secs
            // Note that the arm is visible to help visualize, when animation is added, turn off sprite
            if (leftCooldown < leftStartTime - 0.5f && isBlocking == false)
            {
                leftArm.transform.localPosition = leftOGpos;
            }
        }


    }

    bool HasLeftPunchInput()
    {
        return Input.GetKeyDown(KeyCode.J) && leftDelay == leftStartDelay && isBlocking == false && rightWindUp == false;
    }

    bool HasRightPunchInput()
    {
        return Input.GetKeyDown(KeyCode.K) && rightDelay == rightStartDelay && isBlocking == false && leftWindUp == false;
    }




    bool HasBlockInput()
    {
        // If both arm keys are pressed and the both their delays are greater than 0
        return Input.GetKeyDown(KeyCode.K) && Input.GetKeyDown(KeyCode.J);
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
        rightArm.transform.localPosition = rightOGpos;
        leftArm.transform.localPosition = leftOGpos;
        rightArm.GetComponent<Arm>().SetCollider(b);
        leftArm.GetComponent<Arm>().SetCollider(b);
        subBlockSprite.SetActive(false);
        SetAnimations("block", false);
        SetAnimatorSpeed(1f);
    }

    // Check if the player can start blocking
    bool CheckCanBlock()
    {      
        if (gameObject.GetComponent<Player>().currentStamina < 0)
        {
            return false;
        }
        else if (blockCooldown > 0)
        {
            return false;
        }
        else if(rightDelay <= 0 || leftDelay <= 0)
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
        rightArm.transform.localPosition = rightBlockPos;
        leftArm.transform.localPosition = leftBlockPos;
        rightArm.GetComponent<Arm>().SetCollider(b);
        leftArm.GetComponent<Arm>().SetCollider(b);
        subBlockSprite.SetActive(true);
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

    void RightAttack()
    {
        // Double check if the Player is not blocking
        if (isBlocking == false)
        {
            // Deal damge if enemy is in hitbox
            rightArm.GetComponent<Arm>().SetAttackingTrue();
            rightArm.GetComponent<Arm>().SetCollider(true);

            // Right Arm Attack animation

            // Move arm hitbox into position to deal damage
            if (currentIncrement < incrementTotal)
            {
                if (currentIncrement == 0)
                {
                    gameObject.GetComponent<Unit>().PlayPunchingSound();
                    SetAnimations("right", true);
                }
                currentIncrement += incrementByNum;
                rightArm.transform.localPosition = new Vector3(rightArm.transform.localPosition.x, rightArm.transform.localPosition.y + incrementByNum, rightArm.transform.localPosition.z);
            }
            else
            {
                ResetRightArm();
            }
        }
    }

    void ResetRightArm()
    {
        rightArm.GetComponent<Arm>().SetCollider(false);
        rightArm.GetComponent<Arm>().SetAttackingFalse();

        if (rightArm.GetComponent<Arm>().GetTheEnemy() == null)
        {
            gameObject.GetComponent<Unit>().PlayMissSound();
        }
        rightArm.GetComponent<Arm>().SetTheEnemyNull();

        currentIncrement = 0f;
        // If attacking then reset delay first so that attack only happens once and doesn't loop
        rightDelay = rightStartDelay;
        // Begin cooldown
        rightCooldown = rightStartTime;
    }

    void LeftAttack()
    {
        // Double check if the Player is not blocking
        if (isBlocking == false)
        {
            // Deal damge if enemy is in hitbox
            leftArm.GetComponent<Arm>().SetAttackingTrue();
            leftArm.GetComponent<Arm>().SetCollider(true);

            // Left Arm Attack animation

            // Move arm hitbox into position to deal damage
            if (currentIncrement < incrementTotal)
            {
                if (currentIncrement == 0)
                {
                    gameObject.GetComponent<Unit>().PlayPunchingSound();
                    SetAnimations("left",true);
                }
                currentIncrement += incrementByNum;
                leftArm.transform.localPosition = new Vector3(leftArm.transform.localPosition.x, leftArm.transform.localPosition.y + incrementByNum, leftArm.transform.localPosition.z);
            }
            else
            {
                ResetLeftArm();
            }
        }
    }

    void ResetLeftArm()
    {
        leftArm.GetComponent<Arm>().SetCollider(false);
        leftArm.GetComponent<Arm>().SetAttackingFalse();

        if (leftArm.GetComponent<Arm>().GetTheEnemy() == null)
        {
            gameObject.GetComponent<Unit>().PlayMissSound();
        }
        leftArm.GetComponent<Arm>().SetTheEnemyNull();

        currentIncrement = 0f;
        // If attacking then reset delay first so that attack only happens once and doesn't loop
        leftDelay = leftStartDelay;
        // Begin cooldown
        leftCooldown = leftStartTime;
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
        }
    }

    public void SetAnimatorSpeed(float num)
    {
        animator.SetFloat("BlockAniSpd",num);
    }
}
