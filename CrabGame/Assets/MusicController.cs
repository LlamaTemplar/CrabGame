using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    SoundPlayer soundPlayer;

    private void Awake()
    {
        soundPlayer = GetComponent<SoundPlayer>();
    }

    private void Start()
    {
        playMusic();
    }

    public void playMusic()
    {
        soundPlayer.PlaySound("Ambient");
    }

    public void stopMusic()
    {
        soundPlayer.StopAllSounds();
    }
}
