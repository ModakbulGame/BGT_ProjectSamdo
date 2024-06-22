using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundContainer : MonoBehaviour
{
    private SoundListScript[] m_soundListList;

    public AudioSource GetBGM(EBGM _bgm) { return m_soundListList[(int)ESoundListName.BGM].GetSound((int)_bgm); }
    public void PlayBGM(EBGM _bgm) { m_soundListList[(int)ESoundListName.BGM].PlaySound((int)_bgm); }
    public void PlayPlaySE(EPlaySE _se) { m_soundListList[(int)ESoundListName.PLAYSE].PlaySound((int)_se); }
    public void PlayPlayerSE(EPlayerSE _se) { m_soundListList[(int)ESoundListName.PLAYERSE].PlaySound((int)_se); }
    public void PlaySkillSE(ESkillSE _se) { m_soundListList[(int)ESoundListName.SKILLSE].PlaySound((int)_se); }
    public void PlayMonsterSE(EMonsterSE _se) { m_soundListList[(int)ESoundListName.MONSTERSE].PlaySound((int)_se); }
    public void PlayUISE(EUISE _se) { m_soundListList[(int)ESoundListName.UISE].PlaySound((int)_se); }


    private void Awake()
    {
        m_soundListList = GetComponentsInChildren<SoundListScript>();
    }
}
