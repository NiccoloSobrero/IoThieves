using UnityEngine;

namespace Interactables.ThrowableDistraction
{
    public class PlayerThrower : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Camera cam;
        [SerializeField] private Transform holdPoint;

        [Header("Throw Settings")]
        [SerializeField] private float minPower = 3f;
        [SerializeField] private float maxPower = 15f;
        [SerializeField] private float powerStep = 1f;
        [SerializeField] private float verticalBoost = 4f;

        [SerializeField] private Throwable heldThrowable;
        private float _currentPower;
        private Vector3 _aimDirection;
        
        private Plane _ground;
        
        [Header("Trajectory preview")]
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private int steps = 25;
        [SerializeField] private float timeStep = 0.1f;

        [SerializeField] private GameObject impactPointObj;

        private LayerMask _throwableCollisionMask;

        private bool IsHolding => heldThrowable != null;

        private void Awake()
        {
            if (!cam) cam = Camera.main;
            _currentPower = (minPower + maxPower) * 0.5f;

            _ground = new Plane(Vector3.up, Vector3.zero);
            
            lineRenderer.positionCount = 0;
            
            SetCollisionMask();
        }

        private void Update()
        {
            if (!IsHolding)
            {
                HideTrajectory();
                return;
            }

            UpdateAimDirection();
            Vector3 initialVelocity = ComputeInitialVelocity();

            ShowTrajectory(holdPoint.position, initialVelocity);

            if (Input.GetMouseButtonDown(0))
            {
                Throw();
            }

            // Mouse wheel for power
            if (Input.mouseScrollDelta.y > 0) IncreaseThrowDistance();
            if (Input.mouseScrollDelta.y < 0) DecreaseThrowDistance();
        }
        
        public void TryPickUp(Throwable target)
        {
            if (IsHolding) return;

            heldThrowable = target;
            heldThrowable.OnPickedUp(holdPoint);
        }

        public void Throw()
        {
            if (!IsHolding) return;

            Vector3 initialVelocity = ComputeInitialVelocity();

            heldThrowable.OnThrown();
            heldThrowable.Rb.linearVelocity = initialVelocity;

            heldThrowable = null;
            HideTrajectory();
        }

        public void IncreaseThrowDistance()
        {
            _currentPower = Mathf.Clamp(_currentPower + powerStep, minPower, maxPower);
        }

        public void DecreaseThrowDistance()
        {
            _currentPower = Mathf.Clamp(_currentPower - powerStep, minPower, maxPower);
        }

        private void UpdateAimDirection()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (_ground.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 dir = hitPoint - holdPoint.position;
                dir.y = 0f;
                
                if (dir.sqrMagnitude > 0.01f)
                {
                    _aimDirection = dir.normalized;
                }
            }
            else
            {
                _aimDirection = transform.forward;
                _aimDirection.y = 0f;
                _aimDirection.Normalize();
            }
        }

        private Vector3 ComputeInitialVelocity()
        {
            Vector3 horizontal = _aimDirection * _currentPower;
            Vector3 vertical = Vector3.up * verticalBoost;
            return horizontal + vertical;
        }

        private void SetCollisionMask()
        {
            int layer = heldThrowable.gameObject.layer;

            for (int i = 0; i < 32; i++)
            {
                if (!Physics.GetIgnoreLayerCollision(layer, i))
                {
                    // Add a layer to a LayerMask
                    _throwableCollisionMask |= 1 << i;
                }
            }
        }

        private void ShowTrajectory(Vector3 startPos, Vector3 initialVelocity)
        {
            lineRenderer.positionCount = steps;
            Vector3 pos = startPos;
            Vector3 vel = initialVelocity;
            
            lineRenderer.SetPosition(0, pos);
            
            for (int i = 1; i < steps; i++)
            {
                vel += Physics.gravity * timeStep;
                pos += vel * timeStep;
                
                lineRenderer.SetPosition(i, pos);

                Vector3 lastPos = lineRenderer.GetPosition(i - 1);
                
                if (Physics.Raycast(lastPos, 
                        (pos - lastPos).normalized, 
                        out RaycastHit hit, (pos - lastPos).magnitude, 
                        _throwableCollisionMask))
                {
                    lineRenderer.SetPosition(i, hit.point);
                    lineRenderer.positionCount = i + 1;
                    
                    impactPointObj.transform.position = hit.point;
                    impactPointObj.SetActive(true);
                    
                    return;
                }
            }
            
            impactPointObj.SetActive(false);
        }

        private void HideTrajectory()
        {
            lineRenderer.positionCount = 0;
            impactPointObj.SetActive(false);
        }
    }
}