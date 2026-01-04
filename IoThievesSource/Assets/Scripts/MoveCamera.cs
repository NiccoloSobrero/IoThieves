using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private Camera _mainCamera;
    private Transform _mainCameraTransform;
    private CameraProjectionBlender _cameraProjectionBlender;
    
    [SerializeField] private Transform defaultCameraTransform;
    
    [SerializeField] private List<Transform> cameraTransforms;
    private int _currentCameraIndex;

    private bool _inOrthographicView;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _currentCameraIndex = 0;
        _inOrthographicView = true;

        if (_mainCamera == null) return;
        
        _mainCameraTransform = _mainCamera.transform;
        _cameraProjectionBlender = _mainCamera.GetComponent<CameraProjectionBlender>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_inOrthographicView)
            {
                _cameraProjectionBlender.Perspective();
            }
            else
            {
                _cameraProjectionBlender.Orthographic();
            }
            
            _inOrthographicView = !_inOrthographicView;
        }
        
        if (_inOrthographicView)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                _inOrthographicView = false;
                StopAllCoroutines();
                StartCoroutine(SetToPerspective());
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                // Cycle backwards in the list
                _currentCameraIndex = _currentCameraIndex - 1 < 0 ? cameraTransforms.Count - 1 : _currentCameraIndex - 1;
            
                StopAllCoroutines();
                StartCoroutine(MoveCameraCoroutine(_mainCameraTransform, cameraTransforms[_currentCameraIndex]));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // Cycle forward in the list
                _currentCameraIndex = (_currentCameraIndex + 1) % cameraTransforms.Count;
            
                StopAllCoroutines();
                StartCoroutine(MoveCameraCoroutine(_mainCameraTransform, cameraTransforms[_currentCameraIndex]));
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                _inOrthographicView = true;
                StopAllCoroutines();
                StartCoroutine(ResetToDefault());
            }
        }
    }
    
    private IEnumerator MoveCameraCoroutine(Transform start, Transform end)
    {
        Vector3 startPos = start.position;
        Vector3 endPos = end.position;

        Quaternion startRot = start.rotation;
        Quaternion endRot = end.rotation;

        float eval = 0f;

        while (eval < 1f)
        {
            Vector3 newPos = Vector3.Slerp(startPos, endPos, eval);
            Quaternion newRot = Quaternion.Slerp(startRot, endRot, eval);
            
            _mainCameraTransform.SetPositionAndRotation(newPos, newRot);
            
            eval += Time.deltaTime;
            
            yield return null;
        }
    }
    
    private IEnumerator SetToPerspective()
    {
        _cameraProjectionBlender.Perspective();

        yield return new WaitForSeconds(1f);
        
        yield return StartCoroutine(MoveCameraCoroutine(_mainCameraTransform, cameraTransforms[0]));
    }
    
    private IEnumerator ResetToDefault()
    {
        yield return StartCoroutine(MoveCameraCoroutine(_mainCameraTransform, defaultCameraTransform));
        
        _cameraProjectionBlender.Orthographic();
    }
}
