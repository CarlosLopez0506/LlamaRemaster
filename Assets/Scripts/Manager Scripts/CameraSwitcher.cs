using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public static CameraSwitcher Instance { get; private set; }

    public CinemachineVirtualCamera[] cameras;

    private void Awake()
    {
        // Ensure there is only one instance of CameraSwitcher
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (cameras.Length > 0)
        {
            Debug.Log("Enabling camera with priority: " + cameras[0].Priority);
            cameras[0].enabled = true;
        }
    }

    public void SwitchCamera(CinemachineVirtualCameraBase startingCamera, CinemachineVirtualCameraBase endCamera)
    {
        foreach (var camera in cameras)
        {
            if (camera != startingCamera)
            {
                camera.enabled = false;
            }
        }

        // Enable the new camera
        endCamera.enabled = true;
    }
}