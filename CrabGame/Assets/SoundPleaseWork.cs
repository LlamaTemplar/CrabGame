using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPleaseWork : MonoBehaviour
{
    [ContextMenu("Play random sound")]
    private void PlayRandomSound()
    {
        GetComponent<AudioSource>().Play();
    }
}
