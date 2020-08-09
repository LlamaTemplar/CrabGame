using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { THROWABLE, MELEE};

public class Item : MonoBehaviour
{
    // What type of item is it
    public ItemType type;

    // How much damage does this item deal if any (if none leave at 0)
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If the item has a parent...
        if (transform.parent != null)
        {
            // If the player lost an arm while the item is Parented to it...
            if (transform.parent.GetComponent<Arm>().lostArm == true)
            {
                // Destroy the item
                Destroy(gameObject);
            }
        }
    }

    // Checks what other Objects are colliding with it
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Checks if the object is the Player and an arm
        if (col.gameObject.tag == "Player" && col.gameObject.name.Contains("Arm"))
        {
            // If the arm still has HP and isn't holding anything...
            if (col.gameObject.GetComponent<Arm>().itemInArm == false && col.gameObject.GetComponent<Arm>().lostArm == false)
            {
                // Turn off Collider(because it will use the arms colliders)
                gameObject.GetComponent<Collider2D>().enabled = false;
                // Parent the item to the arm
                transform.SetParent(col.transform, true);
                // Position the item to the Parent
                transform.position = col.transform.position;
            }
        }
    }
}
