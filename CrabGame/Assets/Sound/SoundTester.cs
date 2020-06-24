using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTester : MonoBehaviour
{
    SoundPlayer soundPlayer;

    private void Awake()
    {
        soundPlayer = GetComponent<SoundPlayer>();
    }

    private void Start()
    {
        soundPlayer.PlaySound("Crab");
    }

    [ContextMenu("Play random sound")]
    private void PlayRandomSound()
    {
        soundPlayer.PlaySound("Crab");
        //soundPlayer.PlaySound(Random.Range(0, soundPlayer.GetSoundsLength() - 1));
    }
}
