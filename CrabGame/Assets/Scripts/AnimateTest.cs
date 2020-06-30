using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTest : MonoBehaviour
{
    public Animator animator;
    public bool leftAttack;
    public float timer = 0;
    public bool rightAttack;
    public bool isDefend;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            if (leftAttack)
            {
                leftAttack = false;
                animator.SetBool("IsAttack_LeftClaw", false);
            }

            if (rightAttack)
            {
                rightAttack = false;
                animator.SetBool("IsAttack_RightClaw", false);
            }

            if (isDefend)
            {
                isDefend = false;
                animator.SetBool("IsDefend", false);
            }

            if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.K))
            {
                //Block
                Block();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                //Left Claw
                LeftClaw();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                // Right Claw
                RightClaw();
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
        
    }

    void LeftClaw()
    {
        if (isDefend == false && rightAttack == false)
        {
            leftAttack = true;
            animator.SetBool("IsAttack_LeftClaw", true);
            timer = 2f;
        }
    }

    void RightClaw()
    {
        if (isDefend == false && leftAttack == false)
        {
            rightAttack = true;
            animator.SetBool("IsAttack_RightClaw", true);
            timer = 2f;
        }
    }

    void Block()
    {
        if (leftAttack == false && rightAttack == false)
        {
            isDefend = true;
            animator.SetBool("IsDefend", true);
            timer = 0.5f;
        }
    }
}
