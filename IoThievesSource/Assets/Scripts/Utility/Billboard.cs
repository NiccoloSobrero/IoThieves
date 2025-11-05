using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        GetComponent<Canvas>().worldCamera = _camera;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _camera.transform.forward);
    }
}
