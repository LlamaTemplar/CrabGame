using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    AudioSource audioSource;
    SoundManager soundManager;

    [SerializeField]
    Sound[] sounds;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();

        GameObject soundContainer = new GameObject("Sound Container");
        soundContainer.transform.SetParent(this.transform);

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject gb = new GameObject("Sound_" + i + "_" + sounds[i].clipName);
            gb.transform.SetParent(soundContainer.transform);
            sounds[i].SetSource(gb.AddComponent<AudioSource>());
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

    public void PlaySound(int index)
    {
        sounds[index].Play();
    }

    public int GetSoundsLength()
    {
        return sounds.Length;
    }

}
