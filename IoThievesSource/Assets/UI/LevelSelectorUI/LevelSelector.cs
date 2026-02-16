using Events;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    protected UIDocument uiDocument;

    private void OnEnable()
    {
        EventsManager.OnEnterLevelSelection.AddListener(ShowLevelSelectionUI);
        HideLevelSelectionUI();
    }
    
    private void OnDisable()
    {
        EventsManager.OnEnterLevelSelection.RemoveListener(ShowLevelSelectionUI);
    }

    private void ShowLevelSelectionUI()
    {
        uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        var root = uiDocument.rootVisualElement;
        
        var leftLabel = root.Q<VisualElement>("leftArrowContainer");
        var rightLabel = root.Q<VisualElement>("rightArrowContainer");

        StartPulseAnimation(leftLabel);
        StartPulseAnimation(rightLabel);
    }
    
    private void HideLevelSelectionUI()
    {
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        // PlayerInputManager.Instance.SetContext(InputContext.Gameplay);
    }

    private void StartPulseAnimation(VisualElement element)
    {
        // the higher the duration, the slower the animation
        float duration = 1.8f;
        float minScale = 1f;
        float maxScale = 1.05f;

        element.schedule.Execute(() =>
        {
            float t = (Mathf.Sin(Time.time * (Mathf.PI * 2f / duration)) + 1f) * 0.5f;

            float scale = Mathf.Lerp(minScale, maxScale, t);
            float brightness = Mathf.Lerp(0.65f, 1f, t);

            element.style.scale = new Scale(new Vector3(scale, scale, 1f));
            element.style.opacity = brightness;

        }).Every(0);
    }
}
