using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public float masterVolume = 1f;

    private void Awake()
    {
        // Only allow one SoundManager
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        SetMasterVolume(masterVolume);
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }


}
