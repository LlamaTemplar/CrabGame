using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
	private SoundPlayer soundPlayer;
	private bool missed = false;

	void Start()
	{
		soundPlayer = gameObject.GetComponent<SoundPlayer>();
	}

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
		}
		soundPlayer.PlaySound("Missing");
		missed = true;
	}
}
