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

    private GameObject theEnemy = null;

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
    }

    public void SetAttackingTrue()
    {
        attacking = true;
    }

    public void SetAttackingFalse()
    {
        attacking = false;
        currentDamage = ogDamage;
    }

    public void SetCollider(bool b)
    {
        armCollider.enabled = b;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (gameObject.CompareTag("Player Arm"))
        {
            if (col.gameObject.CompareTag("Enemy Arm") && col.gameObject.GetComponentInParent<EnemyActions>().currentAction == EnemyAction.Block)
            {
                theEnemy = col.gameObject;
                currentDamage = 0;
                // Hey Cole, we can play a sound for when you punch someone guarding here
                col.gameObject.GetComponentInParent<Enemy>().LoseStamina(dmgToStamina);
            }
            else if (col.gameObject.CompareTag("Enemy"))
            {
                theEnemy = col.gameObject;
                if (attacking && col.gameObject.GetComponentInParent<EnemyActions>().currentAction != EnemyAction.Block)
                {
                    col.gameObject.GetComponentInParent<Enemy>().TakeDamage(currentDamage);
                    col.gameObject.GetComponentInParent<Enemy>().TakeKnockBack(transform.parent.position);
                    attacking = false;
                }
            }
        }
        else if (gameObject.CompareTag("Enemy Arm"))
        {
            if (col.gameObject.CompareTag("Player Arm") && col.gameObject.GetComponentInParent<PlayerActions>().isBlocking)
            {
                theEnemy = col.gameObject;
                currentDamage = 0;
                // Hey Cole, we can play a sound for when you punch someone guarding here
                col.gameObject.GetComponentInParent<Player>().LoseStamina(dmgToStamina);
            }
            else if (col.gameObject.CompareTag("Player"))
            {
                theEnemy = col.gameObject;
                if (attacking && col.gameObject.GetComponentInParent<PlayerActions>().isBlocking == false)
                {
                    col.gameObject.GetComponentInParent<Player>().TakeDamage(currentDamage);
                    col.gameObject.GetComponentInParent<Player>().TakeKnockBack(transform.parent.position);
                    attacking = false;
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

    public void TakeDamamge(int dmg)
    {
        if (loseArm == false)
        {
            currentHP -= dmg;
        }
    }
}
