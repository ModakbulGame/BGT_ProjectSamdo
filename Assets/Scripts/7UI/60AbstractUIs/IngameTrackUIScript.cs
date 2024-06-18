using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameTrackUIScript : MonoBehaviour            // 오브젝트에 달려있는 UI
{
    public virtual void DestroyUI()
    {
        Destroy(gameObject);
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