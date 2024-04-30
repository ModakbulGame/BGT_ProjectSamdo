using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private void OnTriggerEnter(Collider _other)
    {
        switch (_other.tag)
        {
            case ValueDefine.MONSTER_ATTACK_TAG:            // ���� ���� ����
                ObjectAttackScript attack = _other.GetComponent<ObjectAttackScript>();
                if(attack == null) { Debug.LogError("���� ��ũ��Ʈ ����"); return; }
                if(!attack.IsAttacking) { return; }
                if(IsDead || attack.CheckHit(this)) { return; }
                Vector3 point = _other.ClosestPoint(Position);
                HitData hit = new(attack.Attacker, attack.Damage, point, attack.CCType);
                GetHit(hit);
                attack.AddHitObject(this);
                break;
            case ValueDefine.CAMERA_TAG:
                CinemachineFreeLook targetCamera = _other.GetComponentInChildren<CinemachineFreeLook>();
                PlayManager.CameraSwitch(targetCamera);
                break;
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        switch(_other.tag)
        {
            case ValueDefine.CAMERA_TAG:
                PlayManager.CameraSwitch(PlayManager.PlayerFreeLook);
                break;
        }
    }
}
