using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{

    public AudioMixerGroup audioMixerGroup;

    private AudioSource source;
    public string clipName;
    public AudioClip clip;

    [Range(0,1f)]
    public float volume;
    [Range(0,3f)]
    public float pitch;

    public bool loop = false;
    public bool playOnAwake = false;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.pitch = pitch;
        source.volume = volume;
        source.playOnAwake = playOnAwake;
        source.loop = loop;
        source.outputAudioMixerGroup = audioMixerGroup;
    }

    public void Play()
    {
        source.Play();
    }

}


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    Sound[] sounds;

    private void Awake()
    {
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
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _gb = new GameObject("Sound_" + i + "_" + sounds[i].clipName);
            _gb.transform.SetParent(this.transform);
            sounds[i].SetSource(_gb.AddComponent<AudioSource>());
        }
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].clipName == _name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

}
