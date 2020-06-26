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
           // gb.transform.SetParent(soundContainer.transform);
            sounds[i].Source = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].clipName == _name)
            {
                //GetComponent<AudioSource>().PlayOneShot(sounds[i].clip);
                //GetComponentInChildren<AudioSource>().Play();
                //transform.GetChild(0).gameObject.AddComponent<AudioSource>().PlayOneShot(sounds[i].clip);
                //sounds[i].Play();
                sounds[i].Source.PlayOneShot(sounds[i].clip);
                return;
            }
        }
    }

    public void PlaySound(int index)
    {
        var s = sounds[index].Source;
        s.PlayOneShot(sounds[index].clip);
        //GetComponent<AudioSource>().PlayOneShot(sounds[index].clip);
    }

    public int GetSoundsLength()
    {
        return sounds.Length;
    }

}
