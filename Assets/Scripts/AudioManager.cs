using UnityEngine.Audio;
using System;
using UnityEngine;

// easily add/remove sounds as we go
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake ()
    {
        foreach (Sound s in sounds)
        {
           s.source = gameObject.AddComponent<AudioSource>();
           s.source.clip = s.clip;
           s.source.volume = s.volume;
           s.source.pitch = s.pitch;
           s.source.loop = s.loop;
        }
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            print("WARNING: sound with name " + name + " not found!");
            return;
        }

        s.source.Play();

    }
}
