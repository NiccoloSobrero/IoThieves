using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndLevelMenuController : MonoBehaviour
{
    [SerializeField]
    protected UIDocument menuDocument;
    [SerializeField]
    private VisualTreeAsset documentCardTemplate;
    [SerializeField]
    private List<string> documents = new List<string>();
    
    private VisualElement endLevelMenuContainerVisualElement;

    private void OnEnable()
    {
        var root = menuDocument.rootVisualElement;
        endLevelMenuContainerVisualElement = root.Q<VisualElement>("endLevelMenuCardsContainer");
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (documentCardTemplate == null)
            return;
        foreach (string document in documents)
        {
            var documentButtonInstance = documentCardTemplate.Instantiate();
            var nameLabel =  documentButtonInstance.Q<Label>("cardDescription");
            nameLabel.text = document;
            endLevelMenuContainerVisualElement.Add(documentButtonInstance);
        }
    }

    public void ActivateEndLevelUI()
    {
        menuDocument.enabled = true;
    }
    
}
