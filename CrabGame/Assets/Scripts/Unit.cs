using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int startingHP = 100;
    public int currentHP;
    protected HealthBar healthBar;
    // Stamina Bar uses Healthbar script
    public int startingStamina = 70;
    public float currentStamina;
    public float staminaNum = 1f;
    protected HealthBar staminaBar;

    public bool isBlocking = false;

    private Animator animator;

    // For Walking Sound
    public SoundPlayer soundPlayer;
    private Vector3 oldPos;
    public bool isWalking = false;
    public bool once = false;
    // For Punching Sound
    public bool isPunching = false;
    // For Being Hit Sound
    private bool beenHit = false;
    // FOr Missing Sound
    private bool missed = false;

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
        currentStamina = startingStamina;
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

        if (staminaBar == null)
        {
            print("Reference to Stamina Bar is Missing");
            print(gameObject);
        }
        else
        {
            staminaBar.SetMaxHealth(startingStamina);
            staminaBar.SetHealth((int)currentStamina);
        }

        if (gameObject.GetComponent<SoundPlayer>() != null)
            soundPlayer = gameObject.GetComponent<SoundPlayer>();
        else
            print("This GameObject Doesn't have a Sound Player Component");

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            print("animator is missing");
        }

        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfWalking();
        PlayWalkingSound(isWalking);
        
        // use a timer insetad of checking if the current pos is at target pos
        // targetpos - currentpos = abs(value) check if less than 0.1
        if (isKnockedBack)
        {
            KeepMoving(transform.position);
        }

        if (staminaBar != null)
        {
            // If NOT blocking then regain Stamina
            if (isBlocking == false && currentStamina < startingStamina)
            {
                currentStamina += staminaNum * Time.deltaTime;
                staminaBar.SetHealth((int)currentStamina);
            }
            else if (isBlocking && currentStamina > 0)
            {
                currentStamina -= staminaNum * Time.deltaTime;
                staminaBar.SetHealth((int)currentStamina);
            }
        }
        //UpdateOldPosition();
    }

    private void FixedUpdate()
    {
        if (animator != null)
        {
            if (isWalking)
            {
                animator.SetBool("IsMove", true);
            }
            else
            {
                animator.SetBool("IsMove", false);
            }
        }
        UpdateOldPosition();
    }

    protected virtual void Die()
    {
		// Que Game over screen
		Destroy(healthBar.gameObject);
    }

    public void TakeDamage(int dmg)
    {
        PlayBeingHitSound();
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

    public void LoseStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
        }
        else
        {
            int diff = (int)(amount - currentStamina);
            TakeDamage(diff);
            currentStamina = 0f;
        }
        staminaBar.SetHealth((int)currentStamina);
    }

    public void SetStaminaBar(HealthBar staminaBar)
    {
        this.staminaBar = staminaBar;
    }

    public HealthBar GetStaminaBar()
    {
        return staminaBar;
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
                print("Walking");
                once = true;
            }
        }
        else
        {
            soundPlayer.StopSound("Walking");
            once = false;
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

    public void PlayBeingHitSound()
    {
        if (beenHit)
        {
            soundPlayer.StopSound("BeingHit");
            beenHit = false;
        }
        soundPlayer.PlaySound("BeingHit");
        beenHit = true;
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
        if (knockTime > 0)
        {
            knockTime -= Time.deltaTime;
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
        else
        {
            knockTime = knockTimeLength;
            isKnockedBack = false;
        }
    }

    public void PlayMissSound()
    {
        if (missed)
        {
            soundPlayer.StopSound("Missing");
            missed = false;
        }
        print("Miss Sound Played");
        soundPlayer.PlaySound("Missing");
        missed = true;
    }
}
