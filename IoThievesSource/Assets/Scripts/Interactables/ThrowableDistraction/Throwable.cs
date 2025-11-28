using UnityEngine;

namespace Interactables.ThrowableDistraction
{
    [RequireComponent(typeof(Rigidbody))]
    public class Throwable : MonoBehaviour
    {
        private Rigidbody _rb;
        public Rigidbody Rb => _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void OnPickedUp(Transform parent)
        {
            _rb.isKinematic = true;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void OnThrown()
        {
            transform.SetParent(null);
            _rb.isKinematic = false;
        }
    }
}