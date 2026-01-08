using System.Collections;
using SuspicionSystem;
using UnityEngine;

namespace Enemy
{
    public class NpcTriggerConeOfDeath : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AlertManager alertManager;
        
        [Header("Sphere")]
        [SerializeField] private float scaleCone = 5.0f;

        private void Awake()
        {
            SetDetectionCone();
        }

        private void SetDetectionCone()
        {
            transform.localScale*=scaleCone; 
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") 
                && IsVisible(other))

            {
                alertManager.ChangeState(alertManager.GameOverState);
                //alertManager.SetAlertedState(true);
            }
        }

        /*
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                alertManager.SetAlertedState(false);
            }
        }*/

        private bool IsVisible(Collider other)
        {
            return !Physics.Raycast(transform.position, (other.transform.position - transform.position),
                Vector3.Distance(transform.position, other.transform.position), LayerMask.GetMask("Default"),QueryTriggerInteraction.Ignore);
        }
        
        
       //----------------------------------------------------
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
           Vector3 endScale = new Vector3(scaleCone, scaleCone * 0.5f, scaleCone);

           while (elapsed < duration)
           {
               elapsed += Time.deltaTime;
               float t = Mathf.Clamp01(elapsed / duration);

               transform.localScale = Vector3.Lerp(startScale, endScale, t);

               yield return null;
           }

           // Ensure exact final value
           transform.localScale = endScale;
       }
    }
}
