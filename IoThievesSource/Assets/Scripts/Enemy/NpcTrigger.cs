using System.Collections;
using SuspicionSystem;
using UnityEngine;

namespace Enemy
{
    public class NpcTrigger : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private AlertManager alertManager;

        [Header("Sphere")] [SerializeField] private float detectionRange;

        private void Awake()
        {
            SetDetectionSphere();
        }

        private void SetDetectionSphere()
        {
            transform.localScale = new Vector3(detectionRange, detectionRange, detectionRange);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                alertManager.SetAlertedState(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                alertManager.SetAlertedState(false);
            }
        }

        //-------------------------------------------------------

        private IEnumerator ShrinkFov()
        {
            float duration = 0.15f;
            float elapsed = 0f;

            Vector3 startScale = transform.localScale;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

                yield return null;
            }

            // Ensure exact final value
            transform.localScale = Vector3.zero;
        }

        
        private IEnumerator EnlargeFov()
        {
            float duration = 0.15f;
            float elapsed = 0f;

            Vector3 startScale = transform.localScale;
            Vector3 endScale = new Vector3(detectionRange,detectionRange,detectionRange);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                transform.localScale = Vector3.Lerp(startScale,endScale, t);

                yield return null;
            }

            // Ensure exact final value
            transform.localScale = endScale;
            
        }
        
    }
}
