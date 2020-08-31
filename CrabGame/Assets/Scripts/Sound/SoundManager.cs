using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public float volumeSetByUser = 1f;
    private float masterVolume = 0f;

    private void Awake()
    {
        // Only allow one SoundManager
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // hack, I'm realizing I need to change how this class functions..
        FadeVolumeIn(10f);
    }

    private void Update()
    {
        SetMasterVolume(masterVolume);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        AudioListener.volume = volume;
    }

    /// <summary>
    /// Fades master volume to desired amount with a given duration. Use this if you want a little more control, 
    /// otherwise it is safer to use <c>FadeVolumeIn</c> and <c>FadeVolumeOut</c>. Make sure there are no other active
    /// fade corounties. 
    /// </summary>
    /// <param name="start">Starting volume. Must be between 0 and 1</param>
    /// <param name="finish">Volume target. Must be beween 0 and 1</param>
    /// <param name="duration">The time it takes to fade in seconds</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator FadeVolume(float start, float finish, float duration)
    {
        
        var timeStart = Time.unscaledTime;
        yield return new WaitUntil(() => {

            var rate = (Time.unscaledTime - timeStart) / duration;
            var volume = Mathf.Lerp(start, finish, rate);

            SetMasterVolume(volume);
            return rate >= 1;
        });

    }

    /// <summary>
    /// Fade current master volume to <c>volumeSetByUser</c>
    /// </summary>
    /// <param name="duration">The time it takes to fade in seconds</param>
    public void FadeVolumeIn(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeVolume(masterVolume, volumeSetByUser, duration));
    }

    /// <summary>
    /// Fades current master volume to zero
    /// </summary>
    /// <param name="duration">The time it takes to fade in seconds</param>
    public void FadeVolumeOut(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeVolume(masterVolume, 0, duration));
    }

}
