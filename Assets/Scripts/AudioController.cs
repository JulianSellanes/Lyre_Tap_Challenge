using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioController : MonoBehaviour
{
    public Sound[] musicSounds;
    public Sound[] sfxSounds;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public static AudioController instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
    }

    public void PlayMusic(string _name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == _name);

        if(sound != null)
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string _name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == _name);

        if (sound != null)
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}