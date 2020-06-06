using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int startingHP = 100;
    public int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = startingHP;
    }

    // Update is called once per frame
    void Update()
    {
        //tenp mmove code
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y, gameObject.transform.position.z);
            //transform.Rotate(0,0,Time.deltaTime * 20);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 0.1f, gameObject.transform.position.y, gameObject.transform.position.z);
            //transform.Rotate(0,0,-Time.deltaTime * 20);
        }
    }

    void Die()
    {
        //Death Animation
        Destroy(gameObject);
        //Que Game over screen
        print("Game Over");
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Die();
        }
    }
}
