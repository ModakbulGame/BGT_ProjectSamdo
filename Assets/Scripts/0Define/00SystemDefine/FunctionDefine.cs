using System;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void FPointer();                                    // �Լ� ������
public delegate void EventPointer(PointerEventData _data);          // ���콺 ���� ���� �Լ� ������

public static class FunctionDefine
{
    public static float Sin(float _angle) { return Mathf.Sin(_angle * Mathf.Deg2Rad); }                 // ���� (������ ��ȯ)
    public static float Cos(float _angle) { return Mathf.Cos(_angle * Mathf.Deg2Rad); }                 // �ڻ��� (������ ��ȯ)

    public static float Round(float _num)                                                             // �ݿø�
    {
        float unit = (float)Mathf.Round(_num);

        if (_num - unit < 0.000001f && _num - unit > -0.000001f) return unit;
        else return _num;
    }
    public static float RoundF2(float _num)
    {
        int n2 = Mathf.RoundToInt(_num * 100);
        return n2 * 0.01f;
    }

    public static void AddEvent(EventTrigger _trigger, EventTriggerType _type, EventPointer _function)  // �̺�Ʈ Ʈ���ſ� �̺�Ʈ �߰�
    {
        EventTrigger.Entry entry = new() { eventID = _type };
        entry.callback.AddListener(data => { _function((PointerEventData)data); });
        _trigger.triggers.Add(entry);
    }

    public static float VecToDeg(Vector2 _vec)              // ����=>���� ��ȯ
    {
        float deg = 90 - Mathf.Atan2(_vec.y, _vec.x) * Mathf.Rad2Deg;
        return deg;
    }
    public static Vector2 DegToVec(float _deg)              // ����=>���� ��ȯ
    {
        if (_deg == 0) return Vector2.up;
        else if(_deg < 180)
        {
            float tan = Mathf.Tan((90-_deg) * Mathf.Deg2Rad);
            return new Vector2(1, tan).normalized;
        }
        else if (_deg == 180) return Vector2.down;
        else
        {
            float tan = Mathf.Tan((90-_deg) * Mathf.Deg2Rad);
            return new Vector2(-1, -tan).normalized;
        }
    }

    public static Vector3 AngleToDir(float _angle)
    {
        float radian = _angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }


    public static Vector2 RotateVector2(Vector2 _vec, float _deg)       // ���� ȸ��
    {
        float deg = -_deg;
        float x = _vec.x;
        float y = _vec.y;
        return new(x * Cos(deg) - y * Sin(deg), x * Sin(deg) + y * Cos(deg));
    }


    public static bool IsTerrain(GameObject _obj)
    {
        return _obj.tag switch
        {
            ValueDefine.TERRAIN_TAG => true,

            _ => false
        };
    }

    public static bool CheckCurAnimation(Animator _anim, int _layer, string _name)        // ���� �ִϸ��̼� Ȯ��
    {
        return _anim.GetCurrentAnimatorStateInfo(_layer).IsName(_name);
    }




    public static void SetTransparent(Material _mat)                    // ���׸��� Transparent ���� (���� ����)
    {
        _mat.SetFloat("_Surface", 1f);
        _mat.SetFloat("_Blend", 0f);

        _mat.SetOverrideTag("RenderType", "Transparent");
        _mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _mat.SetInt("_ZWrite", 0);
        _mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        _mat.SetShaderPassEnabled("ShadowCaster", false);
    }
    public static void SetObaque(Material _mat)                         // ���׸��� Obaque ���� (���� �Ұ���)
    {
        _mat.SetFloat("_Surface", 0f);

        _mat.SetOverrideTag("RenderType", "");
        _mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        _mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        _mat.SetInt("_ZWrite", 1);
        _mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _mat.renderQueue = -1;
        _mat.SetShaderPassEnabled("ShadowCaster", true);
    }
}
