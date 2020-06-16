using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    private int startingHP = 20;
    public float currentHP;
    public bool loseArm = false;
    public bool itemInArm = false;

    // Temp damage (Change value between player and enemies)
    public int damage = 10;
    public bool attacking = false;
    public float attackRadius;
    public LayerMask whatIsEnemy;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = startingHP;
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

    public void SetAttacking()
    {
        attacking = true;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (gameObject.CompareTag("Player"))
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                if (attacking)
                {
                    col.gameObject.GetComponentInParent<Enemy>().TakeDamage(damage);
                    attacking = false;
                }
            }
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            if (col.gameObject.CompareTag("Player"))
            {
                if (attacking)
                {
                    col.gameObject.GetComponentInParent<Player>().TakeDamage(damage);
                    attacking = false;
                }
            }
        }
    }

    public void TakeDamamge(int dmg)
    {
        if (loseArm == false)
        {
            currentHP -= dmg;
        }
    }
}
