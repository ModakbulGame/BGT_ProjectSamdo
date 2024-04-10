using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectScript : EffectScript
{
    private AudioSource[] m_sounds;


    private void PlaySound()
    {
        if(m_sounds.Length == 0) { return; }
        int idx = Random.Range(0, m_sounds.Length);
        m_sounds[idx].Play();
    }


    public override void Awake()
    {
        base.Awake();
        m_sounds = GetComponentsInChildren<AudioSource>();
        foreach(AudioSource audio in m_sounds) { audio.playOnAwake = false; }
    }

    private void Start()
    {
        PlaySound();
    }
}
