using UnityEngine;
using UnityEngine.UIElements;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    protected UIDocument uiDocument;

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;
        
        var leftLabel = root.Q<VisualElement>("leftArrowContainer");
        var rightLabel = root.Q<VisualElement>("rightArrowContainer");

        StartPulseAnimation(leftLabel);
        StartPulseAnimation(rightLabel);
    }

    private void StartPulseAnimation(VisualElement element)
    {
        float duration = 1.8f;   // slower = calmer
        float minScale = 1f;
        float maxScale = 1.05f;

        element.schedule.Execute(() =>
        {
            float t = (Mathf.Sin(Time.time * (Mathf.PI * 2f / duration)) + 1f) * 0.5f;

            float scale = Mathf.Lerp(minScale, maxScale, t);
            float brightness = Mathf.Lerp(0.85f, 1f, t);

            element.style.scale = new Scale(new Vector3(scale, scale, 1f));
            element.style.opacity = brightness;

        }).Every(0);
    }
}
