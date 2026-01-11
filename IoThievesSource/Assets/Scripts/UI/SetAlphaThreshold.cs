using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SetAlphaThreshold : MonoBehaviour
    {
        [SerializeField] [Range(0f, 1f)] private float alphaThreshold = 0.1f;
        
        private void Awake()
        {
            Image image = GetComponent<Image>();
            image.alphaHitTestMinimumThreshold = alphaThreshold;
        }
    }
}
