using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
	protected override void Die()
    {
		base.Die();

		// Death Animation

		Destroy(gameObject);
    }
}
