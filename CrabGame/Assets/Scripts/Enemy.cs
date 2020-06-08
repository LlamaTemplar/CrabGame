using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int startingHP = 100;
    public int currentHP;

    public bool isBlocking = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = startingHP;
    }

    // Update is called once per frame
    void Update()
    {
        //temp for test damage
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //TakeDamage(10);
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isBlocking == false)
        {
            currentHP -= dmg;
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Death Animation
        Destroy(gameObject);
        //Que Game over screen
        print("Game Over");
    }
}
