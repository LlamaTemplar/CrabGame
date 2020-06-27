using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    protected override void Die()
    {
		base.Die();

        // Death Animation

        Destroy(gameObject);
        
        // Que Game over screen
        print("Game Over");
    }
}
