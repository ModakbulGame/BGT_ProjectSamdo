using Cinemachine;
using UnityEngine;

public class CameraController:MonoBehaviour
{
	public GameObject cameraTarget;

    private string m_triggerTag = ValueDefine.CAMERA_TAG;

    public CinemachineFreeLook m_primaryCamera;
    public CinemachineFreeLook[] m_freelookCameras;

    public float rotateSpeed;
	private float rotate;
	public float offsetDistance;
	public float offsetHeight;
	public float smoothing;
	private Vector3 offset;
	private bool following = true;
	private Vector3 lastPosition;

	private void Start()
	{
        SwitchToCamera(m_primaryCamera);

        lastPosition = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y 
			+ offsetHeight, cameraTarget.transform.position.z - offsetDistance);

		offset = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y 
			+ offsetHeight, cameraTarget.transform.position.z - offsetDistance);
	}

	private void Update()
	{
		// Follow, leaves camera at current position.
		if (Input.GetKeyDown(KeyCode.F)) {
			if (following) { following = false; } 
			else { following = true; }
		}

		// Rotate around target.
		if (Input.GetKey(KeyCode.Q)) { rotate = -1; } 
		else if (Input.GetKey(KeyCode.E)) { rotate = 1; } 
		else { rotate = 0; }

		if (following) {
			offset = Quaternion.AngleAxis(rotate * rotateSpeed, Vector3.up) * offset;
			transform.position = cameraTarget.transform.position + offset;
			transform.position = new Vector3(Mathf.Lerp(lastPosition.x, cameraTarget.transform.position.x + offset.x, smoothing * Time.deltaTime),
				Mathf.Lerp(lastPosition.y, cameraTarget.transform.position.y + offset.y, smoothing * Time.deltaTime),
				Mathf.Lerp(lastPosition.z, cameraTarget.transform.position.z + offset.z, smoothing * Time.deltaTime));
		} else { transform.position = lastPosition; }

		transform.LookAt(cameraTarget.transform.position);
	}

	private void LateUpdate()
	{
		lastPosition = transform.position;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_triggerTag))
        {
            CinemachineFreeLook targetCamera = other.GetComponentInChildren<CinemachineFreeLook>();
            SwitchToCamera(targetCamera);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(m_triggerTag))
        {
            SwitchToCamera(m_primaryCamera);
        }
    }

    private void SwitchToCamera(CinemachineFreeLook targetCamera)
    {
        foreach (CinemachineFreeLook camera in m_freelookCameras)
        {
            camera.enabled = camera == targetCamera;
        }
    }
}