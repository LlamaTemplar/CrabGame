using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSoundPlayer : MonoBehaviour
{
    public SoundPlayer soundPlayer;
    private bool enemyDied = false;
    private GameObject player;
    private bool playerOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyDied)
        {
            PlayEnemyDeathSound();
        }

        if (player == null && playerOnce == false)
        {
            playerOnce = true;
            //print("Player Death Sound Played");
            soundPlayer.PlaySound("PlayerDeath");
        }
    }

    private void FixedUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetEnemyDied(bool b)
    {
        enemyDied = b;
    }

    private void PlayEnemyDeathSound()
    {
        if (enemyDied)
        {
            soundPlayer.StopSound("EnemyDeath");
        }
        //print("Enemy Death Sound Played");
        soundPlayer.PlaySound("EnemyDeath");
        enemyDied = false;
    }
}
