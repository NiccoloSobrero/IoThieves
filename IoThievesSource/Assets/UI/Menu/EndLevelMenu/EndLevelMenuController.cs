using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndLevelMenuController : MonoBehaviour
{
    [SerializeField]
    protected UIDocument menuDocument;
    [SerializeField]
    private VisualTreeAsset documentButtonTemplate;
    [SerializeField]
    private List<string> documents = new List<string>();
    
    private VisualElement endLevelMenuContainerVisualElement;

    private void OnEnable()
    {
        var root = menuDocument.rootVisualElement;
        endLevelMenuContainerVisualElement = root.Q<VisualElement>("endLevelMenuContainer");
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (documentButtonTemplate == null)
            return;
        foreach (string document in documents)
        {
            var documentButtonInstance = documentButtonTemplate.Instantiate();
            var nameLabel =  documentButtonInstance.Q<Label>("endLevelDescription");
            nameLabel.text = document;
            endLevelMenuContainerVisualElement.Add(documentButtonInstance);
        }
    }

    public void ActivateEndLevelUI()
    {
        menuDocument.enabled = true;
    }
    
}
