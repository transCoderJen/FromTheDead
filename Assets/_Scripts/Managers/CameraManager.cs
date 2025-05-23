using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineCamera[] _allVirtualCameras;

    [Header("Controls for lerping the Y Damping during player jump/fall")]
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    
    [Header("Controls for the tracked object offset during player jump/fall")]
    [SerializeField] private float _fallOffsetAmount = -2f;
    
    public float _fallSpeedYDampingChangeThreshold = -15f;


    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Coroutine _lerpYPanCoroutine;
    private Coroutine _panCameraCoroutine;

    private CinemachineCamera _currentCamera;
    private CinemachineFollow _positionTransposer;

    private float _normYPanAmount;
    private float _normYOffsetAmount;

    private Vector2 _startingTrackedObjectOffset;

    private void Awake()
    {
        if (instance !=null)
            Destroy(instance.gameObject);
        else
            instance = this;

        for (int i = 0; i < _allVirtualCameras.Length; i++)
        {
            if (_allVirtualCameras[i].enabled)
            {
                // Set the current active camera
                _currentCamera = _allVirtualCameras[i];

                // Set the framing transposer
                _positionTransposer = _currentCamera.GetComponent<CinemachineFollow>();
            }
        }

        // Set the YDamping amount so it's based on the inspector value
        _normYPanAmount = _positionTransposer.TrackerSettings.PositionDamping.y;
        _normYOffsetAmount = _positionTransposer.FollowOffset.y;

        _startingTrackedObjectOffset = _positionTransposer.FollowOffset;
    }

    #region Lerp the Y Damping

    public void LerpYDamping(bool isPlayerFalling)
    {
        // _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
        AnimateFramingTransposer(isPlayerFalling);
    }

    void AnimateFramingTransposer(bool isPlayerFalling)
    {
        var (startOffset, endOffset, startDamp, endDamp) = getStartEndAmounts(isPlayerFalling);
        // Tween m_YDamping
        LeanTween.value(gameObject, startDamp, endDamp, _fallYPanTime)
            .setOnUpdate((float lerpedPanAmount) => 
            {
                _positionTransposer.TrackerSettings.PositionDamping.y = lerpedPanAmount;
            }).setEase(LeanTweenType.easeInOutSine);

        // Tween m_TrackedObjectOffset.y
        LeanTween.value(gameObject, startOffset, endOffset, _fallYPanTime)
            .setOnUpdate((float lerpedOffsetAmount) => 
            {
                var offset = _positionTransposer.FollowOffset;
                offset.y = lerpedOffsetAmount;
                _positionTransposer.FollowOffset = offset;
            }).setEase(LeanTweenType.easeInOutSine);
    }

    private (float startOffsetAmount, float endOffsetAmount, float startDampAmount, float endDampAmount) getStartEndAmounts(bool isPlayerFalling){
        // grab the starting damping amount
        float startDampAmount = _positionTransposer.TrackerSettings.PositionDamping.y;
        float endDampAmount = 0f;

        float startOffsetAmount = _positionTransposer.FollowOffset.y;
        float endOffsetAmount = 0f;

        // determine the end damping amount
        if (isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;

            endOffsetAmount = _fallOffsetAmount;

        }
        else
        {
            endOffsetAmount = _normYOffsetAmount;
            endDampAmount = _normYPanAmount;
        }

        return (startOffsetAmount, endOffsetAmount, startDampAmount, endDampAmount);

    }


    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        // grab the starting damping amount
        float startDampAmount = _positionTransposer.TrackerSettings.PositionDamping.y;
        float endDampAmount = 0f;

        float startOffsetAmount = _positionTransposer.FollowOffset.y;
        float endOffsetAmount = 0f;

        // determine the end damping amount
        if (isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;

            endOffsetAmount = _fallOffsetAmount;

        }
        else
        {
            endOffsetAmount = _normYOffsetAmount;
            endDampAmount = _normYPanAmount;
        }

        // lerp the pan amount
        float elapsedTime = 0f;
        while(elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallYPanTime));
            Debug.Log(lerpedPanAmount);
            _positionTransposer.TrackerSettings.PositionDamping.y = lerpedPanAmount;

            float lerpedOffsetAmount = Mathf.Lerp(startOffsetAmount, endOffsetAmount, (elapsedTime / _fallYPanTime));
            
            _positionTransposer.FollowOffset.y = lerpedOffsetAmount;

            yield return null;
        }

        IsLerpingYDamping = false;
    }

    #endregion

    #region Pan Camera
    
    public void panCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        _panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Debug.Log("Pan Camera");
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        // handle pan from trigger
        if (!panToStartingPos)
        {
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.right;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.left;
                    break;
                default:
                    break;
            }

            endPos *= panDistance;

            startingPos = _startingTrackedObjectOffset;

            endPos += startingPos;
        }

        else
        {
            startingPos = _positionTransposer.FollowOffset;
            endPos = _startingTrackedObjectOffset;
        }

        float elapsedTime = 0f;
        while(elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            _positionTransposer.FollowOffset = panLerp;

            yield return null;
        }


    }
    #endregion

    #region Swap Cameras

    public void SwapCamera(CinemachineCamera cameraFromLeft, CinemachineCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        // if the current camera is the camera on the left and our trigger exit direction was on the right
        if (_currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            // activate the new camera
            cameraFromRight.Priority = 10;

            // deactivate the old camera
            cameraFromLeft.Priority = 0;

           

            // update our composer variable
            _positionTransposer = _currentCamera.GetComponent<CinemachineFollow>();
        }

        // if the current camera is the camera on the right and our trigger exit direction was on the left
        if (_currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
        {
            // activate the new camera
            cameraFromLeft.enabled = true;

            // deactivate the old camera
            cameraFromRight.enabled = false;

           

            // update our composer variable
            _positionTransposer =_currentCamera.GetComponent<CinemachineFollow>();
        }
    }
    #endregion
}
