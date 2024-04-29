using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    #region �ִϸ��̼�
    public float UpperAnimState { get { return m_anim.GetCurrentAnimatorStateInfo(UpperLayerIdx).normalizedTime; } }

    // ���̾� �ε���
    private const int BaseLayerIdx = 0;
    private const int UpperLayerIdx = 1;

    // �ִϸ��̼� �̸�
    private const string UpperIdleAnimName = "PlayerUpperIdle";
    private const string ThrowAnimName = "PlayerUpperThrow";

    // �ؽ� ��
    private int MoveXHash;          // MoveTree X ��
    private int MoveZHash;          // MoveTree Z ��
    private int GuardHash;          // ���� ��
    private int ThrowReadyHash;     // ������ �غ� ��
    private int SkillHash;          // ��ų ��

    public bool IsUpperAnimOn { get { return m_anim.GetLayerWeight(UpperLayerIdx) == 1; } }
    private bool IsUpperIdleAnim { get { return FunctionDefine.CheckCurAnimation(m_anim, UpperLayerIdx, UpperIdleAnimName); } }         // ���� Guard Stop �ִϸ��̼�����
    private bool IsThrowAnim { get { return FunctionDefine.CheckCurAnimation(m_anim, UpperLayerIdx, ThrowAnimName); } }


    // MoveTree X, Z ����
    public void SetMoveAnimation(Vector2 _move) { SetMoveXAnimation(_move.x); SetMoveZAnimation(_move.y); } 
    private void SetMoveXAnimation(float _x) { m_anim.SetFloat(MoveXHash, _x); }
    private void SetMoveZAnimation(float _z) { m_anim.SetFloat(MoveZHash, _z); }


    // Base Layer �ִϸ��̼�
    public void SetIdleAnimator() { SetMoveXAnimation(0); SetMoveZAnimation(0); AttackProcing = false; AttackStack = 0; }          // IDLE
    public void SkillStartAnim() { m_anim.SetBool(SkillHash, true); HideWeapon(); }
    public void SkillAnimDone() { m_anim.SetBool(SkillHash, false); }


    // Upper Layer �ִϸ��̼�
    public void UpperAnimStart() { m_anim.SetLayerWeight(UpperLayerIdx, 1); }
    public void UpperAnimDone() { m_anim.SetLayerWeight(UpperLayerIdx, 0); }

    public void GuardAnimStart() { UpperAnimStart(); m_anim.SetBool(GuardHash, true); }         // ���� ���·� ��ȯ
    public void GuardAnimStop() { m_anim.SetBool(GuardHash, false); }                           // ���� ������ ��ȯ
    public void QuitGuardAnim() { UpperAnimDone(); m_anim.SetBool(GuardHash, false); }          // ���� ��Ŷ�극��ũ

    public void ReadyThrowAnim() { UpperAnimStart(); m_anim.SetBool(ThrowReadyHash, true); }    // ������ �غ�
    public void CancelThrowAnim() { UpperAnimDone(); m_anim.SetBool(ThrowReadyHash, false); }   // ������ ���


    // Ʈ���� �ִϸ��̼�
    public void AttackAnim() { m_anim.SetBool("ATTACK", true); }
    public void SetAttackAnim(int _idx) { m_anim.SetInteger("ATTACK_COUNT", _idx); }
    public void AttackOffAnim() { SetAttackAnim(0); }
    public void SkillFireAnim() { m_anim.SetTrigger("SKILL_FIRE"); }
    public void JumpAnim() { m_anim.SetTrigger("JUMP"); }
    public void RollAnim() { m_anim.SetTrigger("ROLL"); }
    public void ThrowAnim() { m_anim.SetTrigger("THROW"); }
    public void FallAnim() { m_anim.SetTrigger("FALL"); }
    public void LandAnim() { m_anim.SetTrigger("LAND"); }
    public void ResetAnim() { m_anim.SetTrigger("RESET"); }
    public override void HitAnimation()
    {
        AttackOffAnim();
        base.HitAnimation();
    }

    #endregion


    #region ����Ʈ





    #endregion




    // ��� ����
    private void HideWeapon() { CurWeapon.gameObject.SetActive(false); }
    private void ShowWeapon() { CurWeapon.gameObject.SetActive(true); }

    private void SetWeaponAnimationLayer(EWeaponType _type)
    {
        m_anim.SetInteger("WEAPON_IDX", (int)_type);
    }


    private void SetAnimator()          // �ִϸ����� �ؽ� ����
    {
        MoveXHash = Animator.StringToHash("MOVE_X");
        MoveZHash = Animator.StringToHash("MOVE_Z");
        GuardHash = Animator.StringToHash("IS_GUARDING");
        ThrowReadyHash = Animator.StringToHash("IS_THROW_READY");
        SkillHash = Animator.StringToHash("SKILL_ON");
    }
}
