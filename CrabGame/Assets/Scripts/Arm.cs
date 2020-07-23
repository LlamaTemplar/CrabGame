using System.Collections;
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

    // Default is 1
    [Range(1f,10f)]
    public float knockBackDist;
    public float speed = 4f;
    private bool knockedBack = false;
    private GameObject targetToKnock;
    private Vector2 targetPos;

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

        if (targetToKnock != null)
        {
            // targetpos - currentpos = abs(value) check if less than 0.1
            if (targetToKnock.transform.parent.position.x != targetPos.x && targetToKnock.transform.parent.position.y != targetPos.y)
            {
                StartCoroutine(MoveBack(targetToKnock));
            }
            else
            {
                knockedBack = false;
                targetToKnock.transform.parent.GetComponent<Unit>().isKnockedBack = knockedBack;
                StopCoroutine(MoveBack(targetToKnock));
                targetToKnock = null;
            }
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
                    KnockBack(col.gameObject);
                    col.gameObject.transform.parent.GetComponent<Unit>().isKnockedBack = knockedBack;
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
                    KnockBack(col.gameObject);
                    col.gameObject.transform.parent.GetComponent<Unit>().isKnockedBack = knockedBack;
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

    private void KnockBack(GameObject target)
    {
        targetToKnock = target;
        Vector2 diff = target.transform.parent.position - transform.position;

        /*float dist = diff.magnitude;
        // For testing, after hitting the play button, I changed the mass of the target parent and this object's parent to  10 to see changes
        float forceMag = (target.GetComponentInParent<Rigidbody2D>().mass * gameObject.GetComponentInParent<Rigidbody2D>().mass) / Mathf.Pow(dist, 2);*/

        /*Vector2 force = diff.normalized * forceMag;
        // Technically works but Rb for all crabs must have Freeze Positions X and Y turned OFF (unchecked), don't know if thats a problem; Note that the changes 
        // to components are not save and must be inputed manualy
        target.GetComponentInParent<Rigidbody2D>().AddForce(force);*/
       
        //target.transform.parent.position = new Vector2(target.transform.parent.position.x + (diff.x * knockBackDist), target.transform.parent.position.y + (diff.y * knockBackDist));
        targetPos = new Vector2(target.transform.parent.position.x + (diff.x * knockBackDist), target.transform.parent.position.y + (diff.y * knockBackDist));
        
        knockedBack = true;
    }

    IEnumerator MoveBack(GameObject objToMove)
    {
        if (knockedBack)
        {
            // use a timer insetad of checking if the current pos is at target pos
            objToMove.transform.parent.position = Vector2.MoveTowards(objToMove.transform.parent.position, targetPos, speed * Time.deltaTime);
        }
        yield return new WaitForSeconds(0f);
    }
}
