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
        soundPlayer.PlaySound(0);
    }

    [ContextMenu("Play random sound")]
    private void PlayRandomSound()
    {
        soundPlayer.PlaySound(0);
        //soundPlayer.PlaySound(Random.Range(0, soundPlayer.GetSoundsLength() - 1));
    }
}
