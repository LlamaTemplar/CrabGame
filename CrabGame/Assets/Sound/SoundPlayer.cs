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

    /// <summary>
    /// A data container for sounds. All data will be inserted into an <c>AudioSource</c>
    /// when the game starts.
    /// </summary>
    [System.Serializable]
    struct Sound
    {
        
        private AudioSource _source;
        /// <summary>
        /// Reference to the <c>AudioSource</c>
        /// </summary>
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
        soundContainer.transform.localPosition = new Vector3(0, 0, 0);

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject gb = new GameObject("Sound_" + i + "_" + sounds[i].clipName);
            gb.transform.SetParent(soundContainer.transform);
            gb.transform.localPosition = new Vector3(0,0,0);
            sounds[i].Source = gb.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// Plays any of the sounds in <c>sounds</c>
    /// </summary>
    /// <param name="name">The name of the clip</param>
    public void PlaySound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].Source.isPlaying == false && sounds[i].clipName == name)
            {

                //sounds[i].Source.PlayOneShot(sounds[i].clip);
                sounds[i].Source.Play();
                return;
            }
        }
    }

    /// <summary>
    ///  Plays the requested sound in <c>sounds</c>
    /// </summary>
    /// <param name="index">The clips position within the <c>sounds</c> array</param>
    public void PlaySound(int index)
    {
        if (index >= sounds.Length)
            Debug.LogError(gameObject.name + ":  PlaySound request is out of bounds of sounds array)");
        else if(sounds[index].Source.isPlaying == false)
            sounds[index].Source.Play();
    }

    /// <summary>
    /// Stops the requested sound if it's playing
    /// </summary>
    /// <param name="name">The name of the clip</param>
    public void StopSound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].Source.isPlaying && sounds[i].clipName == name)
            {
                sounds[i].Source.Stop();
            }
        }
    }

    /// <summary>
    /// Stops the requested sound if it's playing
    /// </summary>
    /// <param name="index">The clips position within the <c>sounds</c> array</param>
    public void StopSound(int index)
    {
        if(index >= sounds.Length)
        {
            Debug.LogError(gameObject.name + ":  StopSound request is out of bounds of sounds array)");
        }
        else if (sounds[index].Source.isPlaying)
        {
            sounds[index].Source.Stop();
        }
    }

    /// <summary>
    /// Stops all the sounds that are playing
    /// </summary>
    public void StopAllSounds()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].Source.isPlaying)
            {
                sounds[i].Source.Stop();
            }
        }
    }

    public int GetSoundsLength()
    {
        return sounds.Length;
    }

}
