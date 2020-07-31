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

		Destroy(gameObject);
    }

	public void PlayMissSound()
	{
		if (missed)
		{
			soundPlayer.StopSound("Missing");
			missed = false;
		}
		soundPlayer.PlaySound("Missing");
		missed = true;
	}
}
