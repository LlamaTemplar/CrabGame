using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    private Collider2D armCollider;
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
    public float dmgToStamina = 10f;

    public GameObject theEnemy = null;

    // Start is called before the first frame update
    void Start()
    {
        armCollider = GetComponent<Collider2D>();
        armCollider.enabled = false;
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
        //armCollider.enabled = true;
        SetCollider(attacking);
    }

    public void SetAttackingFalse()
    {
        attacking = false;
        //armCollider.enabled = false;
        SetCollider(attacking);
    }

    public void SetCollider(bool b)
    {
        armCollider.enabled = b;
    }

    // Dmage Problem
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (gameObject.CompareTag("Player Arm"))
        {

            if (col.gameObject.CompareTag("Enemy Arm") && col.gameObject.GetComponentInParent<EnemyActions>().isBlocking)
            {
                theEnemy = col.gameObject;
                if (attacking)
                {
                    currentDamage = 0;
                    //return;
                }
            }

            if (col.gameObject.CompareTag("Enemy"))
            {
                theEnemy = col.gameObject;
                if (attacking && col.gameObject.GetComponentInParent<EnemyActions>().isBlocking == false)
                {
                    col.gameObject.GetComponentInParent<Enemy>().TakeDamage(currentDamage);
                    attacking = false;
                    col.gameObject.GetComponentInParent<Unit>().TakeKnockBack(transform.parent.position);
                }
                else if (attacking && col.gameObject.GetComponentInParent<EnemyActions>().isBlocking == true)
                {
                    col.gameObject.GetComponentInParent<Enemy>().TakeDamage(currentDamage);
                    col.gameObject.GetComponentInParent<Enemy>().LoseStamina(dmgToStamina);
                    attacking = false;
                }
            }
        }
        else if (gameObject.CompareTag("Enemy Arm"))
        {
            if (col.gameObject.CompareTag("Player Arm") && col.gameObject.GetComponentInParent<PlayerActions>().isBlocking)
            {
                theEnemy = col.gameObject;
                if (attacking)
                {
                    currentDamage = 0;
                    //print("hit");
                    //return;
                }
            }

            if (col.gameObject.CompareTag("Player"))
            {
                theEnemy = col.gameObject;
                if (attacking && col.gameObject.GetComponentInParent<PlayerActions>().isBlocking == false)
                {
                    col.gameObject.GetComponentInParent<Player>().TakeDamage(currentDamage);
                    attacking = false;
                    col.gameObject.GetComponentInParent<Unit>().TakeKnockBack(transform.parent.position);
                }
                else if (attacking && col.gameObject.GetComponentInParent<PlayerActions>().isBlocking == true)
                {
                    col.gameObject.GetComponentInParent<Player>().TakeDamage(currentDamage);
                    col.gameObject.GetComponentInParent<Player>().LoseStamina(dmgToStamina);
                    attacking = false;
                    //print("hit");
                }
            }
        }
    }

    public GameObject GetTheEnemy()
    {
        return theEnemy;
    }

    public void SetTheEnemyNull()
    {
        theEnemy = null;
    }

    private void OnCollisionStay(Collision col)
    {
        print(col);
    }

    public void TakeDamamge(int dmg)
    {
        if (loseArm == false)
        {
            currentHP -= dmg;
        }
    }
}
