using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
	private LevelManager levelManager;
	private bool isEating = false;

	private void Awake()
	{
		healthBar = GameObject.FindGameObjectWithTag("Player Healthbar").GetComponent<HealthBar>();
		staminaBar = GameObject.FindGameObjectWithTag("Player Staminabar").GetComponent<HealthBar>();
		levelManager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
	}

	protected override void Die()
    {
		base.Die();

		// Death Animation

        Destroy(gameObject);
        
        // Que Game over screen
        print("Game Over");
		levelManager.LoadScene(6);
	}

	public void Heal(int hp)
	{
		currentHP += hp;
		if (currentHP > startingHP)
		{
			currentHP = startingHP;
		}
		GetHealthBar().SetHealth(currentHP);
	}

	public void PlayEatSound()
	{
		if (isEating)
		{
			soundPlayer.StopSound("Eating");
			isEating = false;
		}
		//print("Eating Sound played");
		soundPlayer.PlaySound("Eating");
		isEating = true;
	}
}
