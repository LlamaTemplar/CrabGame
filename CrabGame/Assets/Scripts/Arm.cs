using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    private int startingHP = 20;
    public float currentHP;
    public bool loseArm = false;

    public Collider2D[] opponent;
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

        opponent = Physics2D.OverlapCircleAll(new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z), attackRadius, whatIsEnemy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z), attackRadius);
    }

    public void DealDamage(int dmg)
    {
        for (int i=0; i< opponent.Length; i++)
        {
            opponent[i].GetComponent<Enemy>().TakeDamage(dmg);
            print(opponent[i].gameObject.name);
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
