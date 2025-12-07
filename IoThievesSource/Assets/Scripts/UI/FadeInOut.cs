using System.Collections;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeInOut : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void StartFade(float fadeInTime, float fadeStayTime, float fadeOutTime)
        {
            _canvasGroup.alpha = 0f;
            
            StartCoroutine(FadeCoroutine(fadeInTime, fadeStayTime, fadeOutTime));
        }

        private IEnumerator FadeCoroutine(float fadeInTime, float fadeStayTime, float fadeOutTime)
        {
            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += Time.deltaTime / fadeInTime;

                yield return null;
            }

            yield return new WaitForSeconds(fadeStayTime);
        
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= Time.deltaTime / fadeOutTime;
                
                yield return null;
            }
            
            gameObject.SetActive(false);
        }
    }
}
