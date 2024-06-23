using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameTrackUIScript : MonoBehaviour            // 오브젝트에 달려있는 UI
{
    public virtual void HideUI()
    {
        gameObject.SetActive(false);
    }
    public void ShowUI()
    {
        if (gameObject.activeSelf) { return; }
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