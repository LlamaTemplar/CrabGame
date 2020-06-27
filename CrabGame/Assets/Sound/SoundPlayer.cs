using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : MonoBehaviour
{
    AudioSource audioSource;
    SoundManager soundManager;

    [SerializeField]
    Sound[] sounds;

    [System.Serializable]
    struct Sound
    {
        
        private AudioSource _source;
        [HideInInspector]
        public AudioSource Source
        {
            get { return _source; }
            set
                {
                    _source = value;
                    _source.clip = clip;
                    _source.pitch = pitch;
                    _source.volume = volume;
                    _source.playOnAwake = playOnAwake;
                    _source.loop = loop;
                    _source.outputAudioMixerGroup = audioMixerGroup;
                }
        }

        public string clipName;
        public AudioClip clip;
        public AudioMixerGroup audioMixerGroup;

        [Range(0, 1f)]
        public float volume;
        [Range(0, 3f)]
        public float pitch;

        public bool loop;
        public bool playOnAwake;
    }

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();

        GameObject soundContainer = new GameObject("Sound Container");
        soundContainer.transform.SetParent(this.transform);

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject gb = new GameObject("Sound_" + i + "_" + sounds[i].clipName);
            gb.transform.SetParent(soundContainer.transform);
            sounds[i].Source = gb.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].clipName == _name)
            {

                sounds[i].Source.PlayOneShot(sounds[i].clip);
                return;
            }
        }
    }

    public void PlaySound(int index)
    {
        sounds[index].Source.PlayOneShot(sounds[index].clip);
    }

    public int GetSoundsLength()
    {
        return sounds.Length;
    }

}
