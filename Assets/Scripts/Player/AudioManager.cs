using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] sfxSounds;
    public AudioSource sfxSource;
    public AudioSource bgSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ChangeBgVolume(float volume)
    {
        Instance.bgSource.volume = volume;
    }

    public static void ChangeSFXVolume(float volume)
    {
        Instance.sfxSource.volume = volume;
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name ==name);

        if (s == null )
        {
            Debug.Log("SFX Not Found");
        }

        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
}
