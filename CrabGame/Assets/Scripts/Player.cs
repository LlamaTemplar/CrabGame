using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
	private void Awake()
	{
		healthBar = GameObject.FindGameObjectWithTag("Player Healthbar").GetComponent<HealthBar>();
	}

	protected override void Die()
    {
		base.Die();

        // Death Animation

        Destroy(gameObject);
        
        // Que Game over screen
        print("Game Over");
    }
}
