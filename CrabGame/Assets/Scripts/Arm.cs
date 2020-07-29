﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    private int startingHP = 20;
    public float currentHP;
    public bool loseArm = false;
    public bool itemInArm = false;

    // Change damage value between player and enemies
    public int currentDamage;
    public int ogDamage = 10;
    public bool attacking = false;
    public float attackRadius;
    public LayerMask whatIsEnemy;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = startingHP;
        currentDamage = ogDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            loseArm = true;
        }
        else if (currentHP >= startingHP)
        {
            currentHP = startingHP;
            loseArm = false;
        }

        if (loseArm)
        {
            currentHP += Time.deltaTime;
        }

        if (attacking == false)
        {
            currentDamage = ogDamage;
        }
    }

    public void SetAttackingTrue()
    {
        attacking = true;
    }

    public void SetAttackingFalse()
    {
        attacking = false;
    }

    // Dmage Problem
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (gameObject.CompareTag("Player Arm"))
        {
            if (col.gameObject.CompareTag("Enemy Arm") && col.gameObject.GetComponentInParent<EnemyActions>().isBlocking)
            {
                if (attacking)
                {
                    currentDamage = 0;
                }
            }

            // Currently does not effect arms, it can if you want it to though
            if (col.gameObject.CompareTag("Enemy"))
            {
                if (attacking && col.gameObject.GetComponentInParent<EnemyActions>().isBlocking == false)
                {
                    col.gameObject.GetComponentInParent<Enemy>().TakeDamage(currentDamage);
                    attacking = false;
                    col.gameObject.GetComponentInParent<Unit>().TakeKnockBack(transform.parent.position);
                }
            }
        }
        else if (gameObject.CompareTag("Enemy Arm"))
        {
            if (col.gameObject.CompareTag("Player Arm") && col.gameObject.GetComponentInParent<PlayerActions>().isBlocking)
            {
                if (attacking)
                {
                    currentDamage = 0;
                }
            }

            // Currently does not effect arms, it can if you want it to though
            if (col.gameObject.CompareTag("Player"))
            {
                if (attacking && col.gameObject.GetComponentInParent<PlayerActions>().isBlocking == false)
                {
                    col.gameObject.GetComponentInParent<Player>().TakeDamage(currentDamage);
                    attacking = false;
                    col.gameObject.GetComponentInParent<Unit>().TakeKnockBack(transform.parent.position);
                }
            }
        }
    }

    // Currently Not in use, but kept for reference
    /*private void OnTriggerStay2D(Collider2D col)
    {
        if (gameObject.CompareTag("Player Arm"))
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                if (attacking)
                {
                    
                }
            }
        }
        else if (gameObject.CompareTag("Enemy Arm"))
        {
            if (col.gameObject.CompareTag("Player"))
            {
                if (attacking)
                {
                    
                }
            }
        }
    }*/

    public void TakeDamamge(int dmg)
    {
        if (loseArm == false)
        {
            currentHP -= dmg;
        }
    }
}