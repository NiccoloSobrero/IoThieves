using System;
using System.Collections.Generic;
using Events;
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
        var quitButton = root.Q<Button>("quitButton");
        quitButton.clicked += QuitGame;
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
    
    private void QuitGame()
    {
        EventsManager.OnQuitGame?.Invoke();
    }

    public void ActivateEndLevelUI()
    {
        menuDocument.enabled = true;
    }
    
}
