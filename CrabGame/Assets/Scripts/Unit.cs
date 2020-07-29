﻿using System.Collections;
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

    // For Knock Back
    [Range(0.1f, 1f)]
    public float knockBackDist;
    public float speed = 4f;
    public bool isKnockedBack = false;
    private Vector2 targetPos;
    private float knockTime;
    public float knockTimeLength = 3f;

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

        // use a timer insetad of checking if the current pos is at target pos
        // targetpos - currentpos = abs(value) check if less than 0.1
        if (isKnockedBack)
        {
            if (knockTime > 0)
            {
                KeepMoving(transform.position);
                knockTime -= Time.deltaTime;
            }
            else
            {
                knockTime = knockTimeLength;
            }
        }
    }

	protected virtual void Die()
    {
		// Que Game over screen
		Destroy(healthBar.gameObject);
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        healthBar.SetHealth(currentHP);

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

    public void TakeKnockBack(Vector3 otherPos)
    {
        isKnockedBack = true;
        Vector2 diff = transform.position - otherPos;
        targetPos = new Vector2(transform.position.x + (diff.x * knockBackDist), transform.position.y + (diff.y * knockBackDist));
    }

    public void BeingKnockedBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void KeepMoving(Vector2 unitObject)
    {
        if (Mathf.Abs(targetPos.x - unitObject.x) > 0.1 && Mathf.Abs(targetPos.y - unitObject.y) > 0.1)
        {
            BeingKnockedBack();
        }
        else
        {
            isKnockedBack = false;
            knockTime = 0;
        }
    }
}