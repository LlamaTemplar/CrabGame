using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int startingHP = 100;
    public int currentHP;
    protected HealthBar healthBar;

    public bool isBlocking = false;

    // For Walking Sound
    private SoundPlayer soundPlayer;
    private Vector3 oldPos;
    public bool isWalking = false;
    public bool once = false;
    // For Punching Sound
    public bool isPunching = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = startingHP;
        if (healthBar == null)
        {
            print("Reference to Health Bar is Missing");
            print(gameObject);
        }
        else
        {
            healthBar.SetMaxHealth(startingHP);
            healthBar.SetHealth(currentHP);
        }

        if (gameObject.GetComponent<SoundPlayer>() != null)
            soundPlayer = gameObject.GetComponent<SoundPlayer>();
        else
            print("This GameObject Doesn't have a Sound Player Component");

        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfWalking();
        UpdateOldPosition();
        PlayWalkingSound(isWalking);
    }

	protected virtual void Die()
    {
		// Que Game over screen
		Destroy(healthBar.gameObject);
    }

    public void TakeDamage(int dmg)
    {
        if (isBlocking == false)
        {
            currentHP -= dmg;
            healthBar.SetHealth(currentHP);
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void SetHealthBar(HealthBar healthBar)
    {
        this.healthBar = healthBar;
    }

    public HealthBar GetHealthBar()
    {
        return healthBar;
    }

    private void CheckIfWalking()
    {
        if (oldPos != transform.position)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateOldPosition()
    {
        oldPos = transform.position;
    }

    private void PlayWalkingSound(bool canPlay)
    {
        if (canPlay)
        {
            if (once == false)
            {
                soundPlayer.PlaySound("Walking");
                once = true;
            }
        }
        else
        {
            if (once == true)
            {
                soundPlayer.StopSound("Walking");
                once = false;
            }
        }
    }

    public void PlayPunchingSound()
    {
        if (isPunching)
        {
            soundPlayer.StopSound("Punching");
            isPunching = false;
        }

        soundPlayer.PlaySound("Punching");
        isPunching = true;
    }
}
