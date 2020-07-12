using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTester : MonoBehaviour
{
    SoundPlayer soundPlayer;
    SoundManager soundManager;

    private void Awake()
    {
        soundPlayer = GetComponent<SoundPlayer>();
        soundManager = FindObjectOfType<SoundManager>();    
    }

    private void Start()
    {
        // play a sound
        //soundPlayer.PlaySound("Walk");
    }

    [ContextMenu("Fade audio out")]
    private void FadeVolumeOut()
    {
        // Fade the volume out in 5 seconds
        soundManager.FadeVolumeOut(5);
    }

    [ContextMenu("Fade audio in")]
    private void FadeVolumeIn()
    {
        soundManager.FadeVolumeIn(5);
    }

    [ContextMenu("Play random sound")]
    private void PlayRandomSound()
    {
        soundPlayer.PlaySound(Random.Range(0, soundPlayer.GetSoundsLength()));
    }

    [ContextMenu("Stop all sounds")]
    private void StopAllSounds()
    {
        soundPlayer.StopAllSounds();
    }
}
