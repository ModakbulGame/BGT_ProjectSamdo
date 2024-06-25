using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusScript : MonoBehaviour            // 오브젝트에 달려있는 UI
{
    private void TrackCamRotation()
    {
        float y = Camera.main.transform.localEulerAngles.y;
        transform.localEulerAngles = new(0, y, 0);
    }

    private void LateUpdate()
    {
        TrackCamRotation();
    }
}