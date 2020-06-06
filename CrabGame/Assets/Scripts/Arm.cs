using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    private int startingHP = 20;
    public float currentHP;
    public bool loseArm = false;

    public Collider2D[] enemies;
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

        if (loseArm && gameObject.tag == "Player")
        {
            currentHP += Time.deltaTime;
        }
        else if (loseArm && gameObject.tag == "Enemy")
        {
            //if not dead restore current arms
        }

        enemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, whatIsEnemy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void DealDamage(int dmg)
    {
        for (int i=0; i<enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().TakeDamage(dmg);
            print(enemies[i].gameObject.name);
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
