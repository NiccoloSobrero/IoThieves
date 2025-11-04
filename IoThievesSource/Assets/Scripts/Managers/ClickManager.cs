using System;
using Events;
using Hackables;
using UnityEngine;

namespace Managers
{
    public class ClickManager : MonoBehaviour
    {
        [Header("Raycast Settings")]
        [SerializeField] private float maxRayDistance;
        [SerializeField] private LayerMask interactableLayer;
    
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            EventsManager.OnPointerClick.AddListener(OnPointerClicked);
        }

        private void OnDisable()
        {
            EventsManager.OnPointerClick.RemoveListener(OnPointerClicked);
        }

        private void OnPointerClicked(Vector3 mousePosition)
        {
            Ray ray = _camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, interactableLayer))
            {
                if (hit.transform.TryGetComponent(out HackableDevice hackableDevice))
                {
                    Debug.Log($"Clicked on {hit.collider.gameObject.name}. HackableDevice: {hackableDevice.scrObj.deviceName}");
                    hackableDevice.Hack();
                }
            }
        }
    }
}
