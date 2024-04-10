using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum EEffectName
{
    MONSTER_DISSOLVE,

    LAST
}

public enum EVFXName
{

    LAST
}

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private VisualEffect[] m_visualEffects = new VisualEffect[(int)EVFXName.LAST];
    public VisualEffect GetVFX(EVFXName _vfx) { return m_visualEffects[(int)_vfx]; }


    [SerializeField]
    private GameObject[] m_effects = new GameObject[(int)EEffectName.LAST];
    public GameObject GetEffect(EEffectName _effect) { return m_effects[(int)_effect]; }
}
