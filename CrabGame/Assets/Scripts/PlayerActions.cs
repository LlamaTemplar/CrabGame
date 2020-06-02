using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    //Right Arm variables
    public Collider2D rightArm;
    public float rightCooldown;
    public float rightStartTime = 2f;
    private float rightDelay;
    private float rightStartDelay = 0.2f;
    private bool rightWindUp = false;

    //Left Arm variables 
    public Collider2D leftArm;
    public float leftCooldown;
    public float leftStartTime = 2f;
    private float leftDelay;
    private float leftStartDelay = 0.2f;
    private bool leftWindUp = false;

    //Block variables
    public float blockCooldown;
    public float startBlockTime = 2f;
    private bool isBlocking = false;

    // Start is called before the first frame update
    void Start()
    {
        rightDelay = rightStartDelay;
        leftDelay = leftStartDelay;
    }

    // Update is called once per frame
    void Update()
    {
        //Block code
        //Checks if cooldown is 0
        if (blockCooldown <= 0)
        {
            blockCooldown = 0;
            //if both arm keys are pressed and the both their delays are greater than 0
            if ((Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.F)) && (rightDelay > 0 || leftDelay > 0))
            {
                isBlocking = true;
                //blocking code
                Blocking(isBlocking);
            }
            else if ((Input.GetKeyUp(KeyCode.G) || Input.GetKeyUp(KeyCode.F)) && isBlocking == true) //while blocking, when keys are released undo blocking
            {
                //Blocking cooldown begins counting down
                blockCooldown = startBlockTime;
                isBlocking = false;
                //undoing block code (same code)
                Blocking(isBlocking);
            }
        }
        else
        {
            //Blocking cooldown is counting down
            blockCooldown -= Time.deltaTime;
        }

        //checks if the right arm button is pressed and if the cooldowntime is at 0
        if (rightCooldown <= 0)
        {
            rightCooldown = 0;
            //When key is pressed and the delay is at starting number
            if (Input.GetKeyDown(KeyCode.G) && rightDelay == rightStartDelay) 
            {
                //the wind up is true
                rightWindUp = true;
            }

            //while wind up is true (i don't like while loops)
            if (rightWindUp == true)
            {
                //delay will count down
                rightDelay -= Time.deltaTime;
            }

            //if delay counts down to 0 and Player "is not" blocking, commence attack
            if (rightDelay <= 0 && isBlocking == false)
            {
                rightDelay = 0;
                //reset windup
                rightWindUp = false;

                //Attack code for right arm
                RightAttack();
            }
            else if (rightDelay <= 0 && isBlocking == true) //if delay counts down to 0 and Player "is" blocking, just reset the delay and make windUp false
            {
                rightDelay = rightStartDelay;
                rightWindUp = false;
            }
        }
        else //if cooldown is not 0 then begin countdown (can't attack unless cooldown is at 0)
        {
            //cooldown counting down
            rightCooldown -= Time.deltaTime;
        }

        //checks if the left arm button is pressed and if the cooldowntime is at 0
        if (leftCooldown <= 0)
        {
            leftCooldown = 0;
            //When key is pressed and the delay is at starting number
            if (Input.GetKeyDown(KeyCode.F) && leftDelay == leftStartDelay)
            {
                //the wind up is true
                leftWindUp = true;
            }

            //while wind up is true (i don't like while loops)
            if (leftWindUp == true)
            {
                //delay will count down
                leftDelay -= Time.deltaTime;
            }

            //if delay counts down to 0 and Player "is not" blocking, commence attack
            if (leftDelay <=0 && isBlocking == false)
            {
                leftDelay = 0;
                //reset windup
                leftWindUp = false;

                //Attack code for left arm
                LeftAttack();
            }
            else if (leftDelay <= 0 && isBlocking == true)//if delay counts down to 0 and Player "is" blocking, just reset the delay and make windUp false
            {
                leftDelay = leftStartDelay;
                leftWindUp = false;
            }
        }
        else //if cooldown is not 0 then begin countdown (can't attack unless cooldown is at 0)
        {
            //cooldown counting down
            leftCooldown -= Time.deltaTime;
        }
    }

    void Blocking(bool b)
    {
        if (b == true)
        {
            //Actual Block code
            print("holding block");
        }
        else
        {
            //stops blocking
            print("not blocking");
        }
    }

    void RightAttack()
    {
        //if attacking then reset delay first so that attack only happens once and doesn't loop
        rightDelay = rightStartDelay;
        //double check if the Player is not blocking
        if (isBlocking == false)
        {
            //Actual Attacking
            print("right attack");
            //begin cooldown
            rightCooldown = rightStartTime;
        }
    }

    void LeftAttack()
    {
        //if attacking then reset delay first so that attack only happens once and doesn't loop
        leftDelay = leftStartDelay;
        //double check if the Player is not blocking
        if (isBlocking == false)
        {
            //Actual Attacking
            print("left attack");
            //begin cooldown
            leftCooldown = leftStartTime;
        }
    }
}
