using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class BossAudioManager : MonoBehaviour
{
    [Range(0f, 10f)]
    public float footStepvolumn = 1f;

    public Sound[] sounds;
    public Sound[] footsteps;
    
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = 1;
        }
        foreach (Sound s in footsteps)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = footStepvolumn;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = 1;
        }
        StartCoroutine(WalkSound());
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.source.Play();
    }

    public void PlayFootstep()
    {
        int rand = Random.Range(0, footsteps.Length);
        Sound s = footsteps[rand];
        s.source.Play();
    }

    IEnumerator WalkSound()
    {
        float wait = 0.8f;
        while(true)
        {
            Debug.Log("walk");
            PlayFootstep();
            yield return new WaitForSeconds(wait);
        }
    }
}
