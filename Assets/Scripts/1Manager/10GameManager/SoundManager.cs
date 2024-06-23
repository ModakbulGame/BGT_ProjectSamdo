using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESoundListName
{
    BGM,                        // 배경 음악
    PLAYSE,                     // 게임 진행 효과음
    PLAYERSE,                   // 스킬 제외 플레이어 효과음
    SKILLSE,                    // 플레이어 스킬 효과음
    MONSTERSE,                  // 몬스터 효과음
    UISE,                       // UI 효과음
    LAST
}

public class SoundManager : MonoBehaviour
{
    private SoundContainer m_soundContainer;            // 소리 저장소

    [SerializeField]
    private AudioClip[] m_seList;                       // 개별 프리펍에 없는 소리

    private AudioSource CurBGM { get; set; }

    [Range(0, 100)]
    private static int m_bgmVolume = 100;               // BGM 볼륨
    [Range(0, 100)]
    private static int m_seVolume = 80;                // 효과음 볼륨
    public static float BGMVolume { get { return m_bgmVolume * 0.01f; } }
    public static float SEVolume { get { return m_seVolume * 0.01f; } }
    public void SetBGMVolume(int _volume) { m_bgmVolume = _volume; if (CurBGM) { CurBGM.volume = _volume / 100f; } PlayerPrefs.SetInt(ValueDefine.BGMVolData, _volume); }
    public void SetSEVolume(int _volume) { m_seVolume = _volume; PlayerPrefs.SetInt(ValueDefine.SEVolData, _volume); }

    public void PlayBGM(EBGM _bgm)
    {
        StopBGM();
        CurBGM = m_soundContainer.GetBGM(_bgm);
        CurBGM.volume = BGMVolume;
        CurBGM.Play();
    }
    public void StopBGM() { if (CurBGM != null) CurBGM.Stop(); }

    public void PlaySE(int _idx, Vector3 _point)                        // 효과음 예시
    {
        AudioSource.PlayClipAtPoint(m_seList[_idx], _point, SEVolume);
    }




    public void PlaySE(EPlaySE _se) { m_soundContainer.PlayPlaySE(_se); }
    public void PlaySE(EPlayerSE _se) { m_soundContainer.PlayPlayerSE(_se); }
    public void PlaySE(ESkillSE _se) { m_soundContainer.PlaySkillSE(_se); }
    public void PlaySE(EMonsterSE _se) { m_soundContainer.PlayMonsterSE(_se); }
    public void PlaySE(EUISE _se) { m_soundContainer.PlayUISE(_se); }


    private void Awake()
    {
        m_bgmVolume = PlayerPrefs.GetInt(ValueDefine.BGMVolData, 100);
        m_seVolume = PlayerPrefs.GetInt(ValueDefine.SEVolData, 100);
    }
}
