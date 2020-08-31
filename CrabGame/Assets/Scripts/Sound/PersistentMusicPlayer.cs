using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentMusicPlayer : SoundPlayer
{
    private static PersistentMusicPlayer Instance;
    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        PlaySound(0);
    }
}
