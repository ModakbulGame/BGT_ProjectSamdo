using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlateUIScript : MonoBehaviour
{
    private Animator m_anim;
    private TextMeshProUGUI m_slateTxt;


    private SlateScript CurSlate { get; set; }


    public void ShowSlateText(SlateScript _slate)
    {
        if (CurSlate == null) { CurSlate = _slate; }
        m_anim.SetBool("IS_SHOWING", true);
        m_slateTxt.text = _slate.SlateText;
        StartCoroutine(DisplayCoroutine());
    }
    private IEnumerator DisplayCoroutine()
    {
        yield return new WaitForSeconds(CurSlate.DisplayTime);
        m_anim.SetBool("IS_SHOWING", false);
    }

    public void DisplayDone()
    {
        CurSlate.DisplayDone();
    }


    private void SetComps()
    {
        m_anim = GetComponent<Animator>();
        m_slateTxt = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Awake()
    {
        SetComps();
    }
}
