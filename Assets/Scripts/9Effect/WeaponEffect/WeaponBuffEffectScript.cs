using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponBuffEffectScript : MonoBehaviour
{
    [SerializeField]
    private VisualEffect[] m_effects;

    private readonly float[] EffectHeight = new float[3] { 0.4f, 0.4f, 0.15f };
    private readonly float[] SizePerWeapon = new float[3] { 0.4f, 0.1f, 0.5f };

    private ECCType CurCC { get; set; } = ECCType.LAST;

    private int CC2Idx(ECCType _cc) { return (int)_cc - 1; }

    public void InitWeapon(EWeaponType _type)
    {
        foreach (VisualEffect effect in m_effects)
        {
            float size = SizePerWeapon[(int)_type];
            effect.SetVector3("TrailSize", new(size, EffectHeight[(int)_type], size));
        }
    }

    public void EffectOn(ECCType _cc)
    {
        if(_cc == 0 || _cc > ECCType.EXTORTION) { Debug.LogError("이펙트 없는 CC 입력"); return; }
        if(CurCC != ECCType.LAST && CurCC != _cc) { EffectOff(); }
        CurCC = _cc;
        VisualEffect effect = m_effects[CC2Idx(_cc)];
        effect.enabled = true;
        effect.Play();
    }
    public void EffectOff()
    {
        VisualEffect effect = m_effects[CC2Idx(CurCC)];
        if (!effect.enabled) { return; }
        effect.Stop();
        StartCoroutine(DisableCoroutine(effect));
    }
    private IEnumerator DisableCoroutine(VisualEffect _effect)
    {
        yield return new WaitForSeconds(1);
        _effect.enabled = false;
    }
}
