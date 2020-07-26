using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
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
    private float blockDuration;
    public float blockStartingDuration = 2f;
    public bool isBlocking = false;
    // Will most likely be removed these 3 lines after adding animations
    private Vector3 rightBlockPos;
    private Vector3 leftBlockPos; 
    public GameObject subBlockSprite;

    // Start is called before the first frame update
    void Start()
    {
        blockDuration = blockStartingDuration;

        // Delays for the attacks
        rightDelay = rightStartDelay;
        leftDelay = leftStartDelay;

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
        // Block code
        // Checks if cooldown is 0
        if (blockCooldown <= 0)
        {
            blockCooldown = 0;
            // Check if we still have both arms for blocking
            if (leftArm.GetComponent<Arm>().loseArm == false && rightArm.GetComponent<Arm>().loseArm == false)
            {
                // If both arm keys are pressed and the both their delays are greater than 0
                if ((Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.J)) && (rightDelay > 0 || leftDelay > 0))
                {
                    if (blockDuration > 0)
                    {
                        isBlocking = true;
                        // Blocking code
                        Blocking(isBlocking);
                    }
                    else if (blockDuration <= 0)
                    {
                        // Blocking cooldown begins counting down
                        blockCooldown = startBlockTime;
                        isBlocking = false;

                        blockDuration = blockStartingDuration;
                        // Undoing block code (same code)
                        Blocking(isBlocking);
                    }
                }
                else if ((Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.J)) && isBlocking == true) // While blocking, when keys are released undo blocking
                {
                    // Blocking cooldown begins counting down
                    blockCooldown = startBlockTime;
                    isBlocking = false;

                    blockDuration = blockStartingDuration;
                    // Undoing block code (same code)
                    Blocking(isBlocking);
                }
            }
            else // If arm(s) is missing while blocking then undo blocking
            {
                isBlocking = false;
                Blocking(isBlocking);
            }
        }
        else
        {
            // Blocking cooldown is counting down
            blockCooldown -= Time.deltaTime;
        }

        // Checks if the right arm button is pressed and if the cooldowntime is at 0
        if (rightCooldown <= 0)
        {
            rightCooldown = 0;
            // Check if we still have right arm
            if (rightArm.GetComponent<Arm>().loseArm == false)
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
                rightArm.GetComponent<Arm>().SetAttackingFalse();
            }
        }

        // Checks if the left arm button is pressed and if the cooldowntime is at 0
        if (leftCooldown <= 0)
        {
            leftCooldown = 0;
            // Check if we still have left arm
            if (leftArm.GetComponent<Arm>().loseArm == false)
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
                leftArm.GetComponent<Arm>().SetAttackingFalse();
            }
        }
    }

    void Blocking(bool b)
    {
        if (b == true)
        {
            // Make player bool true, to prevent taking damage
            gameObject.GetComponent<Player>().isBlocking = b;
            // Blocking Animation
            rightArm.transform.localPosition = rightBlockPos;
            leftArm.transform.localPosition = leftBlockPos;
            subBlockSprite.SetActive(true);
            blockDuration -= Time.deltaTime;
        }
        else
        {
            // Make player bool false, to prevent taking damage
            gameObject.GetComponent<Player>().isBlocking = b;
            // Stop Blocking Animation
            rightArm.transform.localPosition = rightOGpos;
            leftArm.transform.localPosition = leftOGpos;
            subBlockSprite.SetActive(false);
        }
    }

    void RightAttack()
    {
        // If attacking then reset delay first so that attack only happens once and doesn't loop
        rightDelay = rightStartDelay;
        // Double check if the Player is not blocking
        if (isBlocking == false)
        {
            // Right Arm Attack animation

            // Move arm hitbox into position to deal damage
            rightArm.transform.localPosition = new Vector3(rightArm.transform.localPosition.x, rightArm.transform.localPosition.y + 0.08f, rightArm.transform.localPosition.z);

            // Deal damge if enemy is in hitbox
            rightArm.GetComponent<Arm>().SetAttackingTrue();

            gameObject.GetComponent<Unit>().PlayPunchingSound();

            // Begin cooldown
            rightCooldown = rightStartTime;
        }
    }

    void LeftAttack()
    {
        // If attacking then reset delay first so that attack only happens once and doesn't loop
        leftDelay = leftStartDelay;
        // Double check if the Player is not blocking
        if (isBlocking == false)
        {
            // Left Arm Attack animation

            // Move arm hitbox into position to deal damage
            leftArm.transform.localPosition = new Vector3(leftArm.transform.localPosition.x, leftArm.transform.localPosition.y + 0.08f, leftArm.transform.localPosition.z);

            // Deal damge if enemy is in hitbox
            leftArm.GetComponent<Arm>().SetAttackingTrue();

            gameObject.GetComponent<Unit>().PlayPunchingSound();

            // Begin cooldown
            leftCooldown = leftStartTime;
        }
    }
}
