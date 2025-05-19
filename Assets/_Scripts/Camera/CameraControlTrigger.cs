using UnityEngine;
using UnityEditor;
using Unity.Cinemachine;
using Unity.VisualScripting;

public class CameraControlTrigger : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;

    private Collider2D _coll;

    private void Start()
    {
        _coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (customInspectorObjects.panCameraOnContact)
            {
                // pan the camera
                CameraManager.instance.panCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Vector2 exitDirection = (collision.transform.position - _coll.bounds.center).normalized;

            if (customInspectorObjects.swapCameras && customInspectorObjects.cameraOnLeft != null && customInspectorObjects.cameraOnRight != null)
            {
                if (exitDirection.x < 0)
                {
                    Debug.Log("Swapping to left camera");
                    // swap to left camera
                    customInspectorObjects.cameraOnLeft.Priority = 10;
                    customInspectorObjects.cameraOnRight.Priority = 0;
                }
                else
                {
                    // swap to right camera
                    customInspectorObjects.cameraOnLeft.Priority = 0;
                    customInspectorObjects.cameraOnRight.Priority = 10;
                }
            }
            if (customInspectorObjects.panCameraOnContact)
            {
                // pan the camera
                CameraManager.instance.panCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
            }
        }
    }
}

[System.Serializable]
public class CustomInspectorObjects
{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;

    [HideInInspector] public CinemachineCamera cameraOnLeft;
    [HideInInspector] public CinemachineCamera cameraOnRight;

    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}

public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}