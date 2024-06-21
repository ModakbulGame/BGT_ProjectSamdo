using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameTrackUIScript : MonoBehaviour            // ������Ʈ�� �޷��ִ� UI
{
    public virtual void HideUI()
    {
        gameObject.SetActive(false);
    }
    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    private void TrackCamRotation()
    {
        transform.forward = Camera.main.transform.forward;
    }

    private void LateUpdate()
    {
        TrackCamRotation();
    }
}