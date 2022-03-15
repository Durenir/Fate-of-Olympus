using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public Sounds[] sounds;
    public static AudioManager instance;
    public float musicVolume;
    public float sfxVolume;
    public bool hasEnded = true;
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Start()
    {
        GameEvents.SaveInitiated += SaveSettings;
    }

    void SaveSettings()
    {
        float[] settings = new float[2];
        settings[0] = musicVolume;
        settings[1] = sfxVolume;
        SaveSystem.Save(settings, "AudioSettings");
    }

    public void LoadSettings()
    {
        if(SaveSystem.SaveExists("AudioSettings"))
        {
            float[] settings = SaveSystem.Load<float[]>("AudioSettings");
            GameManager.instance.musicVolume.value= settings[0];
            GameManager.instance.sfxVolume.value = settings[1];
        }
    }

    void Update()
    {
        foreach (Sounds s in sounds)
        {
            if(s.isMusic)
            {
                s.source.volume = musicVolume;
            } else if(s.isSFX)
            {
                s.source.volume = sfxVolume;
            }
        }
    }
    public void Play(String name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {   
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if(s.isMusic)
            StartCoroutine(StartFade(s.source, 1f));
        else
            s.source.Play();
    }

    public void Stop(String name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {   
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if(s.isMusic)
            StartCoroutine(EndFade(s.source, 1f));
        else
            s.source.Stop();
    }

    public static IEnumerator StartFade(AudioSource source, float duration)
    {
        while(!AudioManager.instance.hasEnded)
        {
            yield return new WaitForSeconds(0.1f);
        }
        AudioManager.instance.hasEnded = false;
        source.volume = 0;
        float currentTime = 0;
        source.Play();
        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            source.volume = Mathf.Lerp(0, AudioManager.instance.musicVolume, currentTime/duration);
            yield return null;
        }
        yield break;
    }
    public static IEnumerator EndFade(AudioSource source, float duration)
    {
        float currentTime = 0;
        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            source.volume = Mathf.Lerp(AudioManager.instance.musicVolume, 0f, currentTime/duration);
            yield return null;
        }
        source.Stop();
        source.volume = AudioManager.instance.musicVolume;
        AudioManager.instance.hasEnded = true;
        yield break;
    }
}
