using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundListScript : MonoBehaviour
{
    [SerializeField]
    protected ESoundListName m_listName;

    protected AudioSource[] m_soundList;

    public AudioSource GetSound(int _idx) { return m_soundList[_idx]; }
    public void PlaySound(int _idx)
    {
        if (m_listName == ESoundListName.BGM)
            m_soundList[_idx].volume = SoundManager.BGMVolume / 100f;
        else
            m_soundList[_idx].volume = SoundManager.SEVolume / 100f;
        m_soundList[_idx].Play();
    }

    public virtual bool ChkList { get { return true; } }

    private void Awake()
    {
        m_soundList = GetComponents<AudioSource>();
    }

    private void Start()
    {
        if (m_soundList == null || !ChkList)
        {
            Debug.LogError("효과음 수가 맞지 않습니다.");
        }
    }
}
