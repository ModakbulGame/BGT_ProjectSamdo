using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour, IInteractable, IHaveData
{
    [SerializeField]
    protected NPCScriptable m_scriptable;
    public void SetScriptable(NPCScriptable _scriptable) { m_scriptable = _scriptable; }

    public Vector2 Position2 { get { return new(transform.position.x, transform.position.z); } }

    public SNPC NPC { get { return m_scriptable.NPC; } }
    public string NPCName { get { return m_scriptable.NPCName; } }
    public DialLine[] DefaultLine { get { return new DialLine[1] { new(m_scriptable.DefaultLine) }; } }

    public List<DialogueScriptable> DialogueList { get { return m_scriptable.DialogueList; } }
    private int DialCount { get { if (m_scriptable == null) { return 0; } return DialogueList.Count; } }
    private bool[] m_dialDone;

    private int AbleDialIdx { get { for (int i = 0; i<DialCount; i++) { if (!m_dialDone[i]) return i; } return -1; } }

    public EInteractType InteractType { get { return EInteractType.NPC; } }
    public virtual string InfoTxt { get { return "대화"; } }            // 상호작용 UI에 띄울 말 => 말이 상황에 따라 바뀌는 경우 조건문 추가 


    public void LoadData()
    {
        GameManager.RegisterData(this);
        if (PlayManager.IsNewData) { m_dialDone = new bool[DialCount]; return; }

        SaveData data = PlayManager.CurSaveData;

        foreach (NPCSaveData save in data.NPCData)
        {
            if(save.NPC != NPC) { continue; }
            bool[] states = save.DialDone;
            for(int i = 0; i<states.Length; i++) { m_dialDone[i] = states[i]; }
            return;
        }
    }

    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        foreach (NPCSaveData save in data.NPCData)
        {
            if (save.NPC != NPC) { continue; }
            bool[] states = save.DialDone;
            for (int i = 0; i<states.Length; i++) { m_dialDone[i] = states[i]; }
            return;
        }

        NPCSaveData newSave = new(NPC, m_dialDone);
        data.NPCData.Add(newSave);
    }

    public virtual void StartDialogue()
    {
        PlayManager.OpenDialogueUI(this, AbleDialIdx);
        m_dialDone[AbleDialIdx] = true;
    }


    public virtual void StartInteract()
    {
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
        StartDialogue();
    }

    public virtual void StopInteract()
    {
        PlayManager.StopPlayerInteract();
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
    }

    private void Start()
    {
        LoadData();
    }
}
