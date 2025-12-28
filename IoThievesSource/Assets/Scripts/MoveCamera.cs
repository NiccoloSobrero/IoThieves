using System.Collections;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform bossHouseCameraTransform;
    [SerializeField] private Transform hrOfficeCameraTransform;
    [SerializeField] private Transform tutorialCameraTransform;

    [SerializeField] private Transform mainCamera;
    
    private Transform _defaultCameraTransform;

    private void Awake()
    {
        _defaultCameraTransform = mainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            mainCamera.GetComponent<Camera>().orthographic = false;
            StopAllCoroutines();
            StartCoroutine(MoveCameraCoroutine(mainCamera, bossHouseCameraTransform));
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            mainCamera.GetComponent<Camera>().orthographic = false;
            StopAllCoroutines();
            StartCoroutine(MoveCameraCoroutine(mainCamera, hrOfficeCameraTransform));
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            mainCamera.GetComponent<Camera>().orthographic = false;
            StopAllCoroutines();
            StartCoroutine(MoveCameraCoroutine(mainCamera, tutorialCameraTransform));
        }
        // Reset
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            mainCamera.GetComponent<Camera>().orthographic = true;
            mainCamera.SetPositionAndRotation(_defaultCameraTransform.position, _defaultCameraTransform.rotation);
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
            
            mainCamera.SetPositionAndRotation(newPos, newRot);
            
            eval += Time.deltaTime;
            
            yield return null;
        }
    }
}
