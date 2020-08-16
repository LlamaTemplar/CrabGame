using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
	private bool missed = false;

	protected override void Die()
    {
		base.Die();

		// Death Animation
		GameObject deathPlayer = GameObject.Find("Death Sound Player");
		if (deathPlayer)
			deathPlayer.GetComponent<DeathSoundPlayer>().SetEnemyDied(true);
		Destroy(gameObject);
    }

}
