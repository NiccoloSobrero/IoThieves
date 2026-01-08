using System;
using System.Collections.Generic;
using Events;
using Minigames;
using UnityEngine;
using UnityEngine.UIElements;

public class EndLevelMenuController : MonoBehaviour
{
    [SerializeField]
    protected UIDocument menuDocument;
    [SerializeField]
    private VisualTreeAsset documentCardTemplate;
    // [SerializeField]
    // private List<string> documents = new List<string>();
    
    private VisualElement endLevelMenuContainerVisualElement;

    private void OnEnable()
    {
        var root = menuDocument.rootVisualElement;
        var quitButton = root.Q<Button>("quitButton");
        quitButton.clicked += QuitGame;
        endLevelMenuContainerVisualElement = root.Q<VisualElement>("endLevelMenuCardsContainer");
        UpdateUI();
        EventsManager.OnEndLevel.AddListener(ShowEndLevelMenu);
    }
    
    // private void OnEnable()
    // {
    //     var root = menuDocument.rootVisualElement;
    //
    //     root.RegisterCallback<GeometryChangedEvent>(evt =>
    //     {
    //         endLevelMenuContainerVisualElement = root.Q<VisualElement>("endLevelMenuCardsContainer");
    //
    //         var quitButton = root.Q<Button>("quitButton");
    //         if (quitButton != null)
    //             quitButton.clicked += QuitGame;
    //
    //         UpdateUI();
    //     });
    // }


    private void UpdateUI()
    {
        if (documentCardTemplate == null)
            return;
        menuDocument.rootVisualElement.style.display = DisplayStyle.None;
    }
    
    // private void UpdateUI()
    // {
    //     if (documentCardTemplate == null)
    //     {
    //         Debug.LogError("Card Template is NULL!");
    //         return;
    //     }
    //
    //     if (endLevelMenuContainerVisualElement == null)
    //     {
    //         Debug.LogError("Container is NULL!");
    //         return;
    //     }
    //     
    //     endLevelMenuContainerVisualElement.Clear();
    //
    //     foreach (string document in documents)
    //     {
    //         var documentButtonInstance = documentCardTemplate.Instantiate();
    //         if (documentButtonInstance == null)
    //         {
    //             Debug.LogError("Instantiate() returned NULL!");
    //             return;
    //         }
    //
    //         var nameLabel = documentButtonInstance.Q<Label>("cardDescription");
    //         nameLabel.text = document;
    //
    //         endLevelMenuContainerVisualElement.Add(documentButtonInstance);
    //     }
    // }

    
    private void QuitGame()
    {
        menuDocument.rootVisualElement.style.display = DisplayStyle.None;
        EventsManager.OnQuitGame?.Invoke();
    }

    private void ShowEndLevelMenu()
    {
        print("Showing End Level");
        
        foreach (FileDetails document in DownloadsManager.Instance.DownloadedFiles)
        {
            var documentButtonInstance = documentCardTemplate.Instantiate();
            var nameLabel =  documentButtonInstance.Q<Label>("cardTitle");
            nameLabel.text = document.filePath;
            var image = documentButtonInstance.Q<Image>("cardImage");
            image.sprite = document.documentSprite;
            endLevelMenuContainerVisualElement.Add(documentButtonInstance);
        }
        
        menuDocument.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    // public void ActivateEndLevelUI()
    // {
    //     menuDocument.enabled = true;
    // }
    
    // public void ActivateEndLevelUI()
    // {
    //     menuDocument.enabled = true;
    //
    //     var root = menuDocument.rootVisualElement;
    //     if (root == null)
    //     {
    //         // UIDocument not ready yet, delay until root exists
    //         StartCoroutine(WaitForRootAndInitialize());
    //     }
    //     else
    //     {
    //         InitializeEndLevelUI(root);
    //     }
    // }

    // private System.Collections.IEnumerator WaitForRootAndInitialize()
    // {
    //     // Wait until rootVisualElement is ready
    //     while (menuDocument.rootVisualElement == null)
    //         yield return null;
    //
    //     InitializeEndLevelUI(menuDocument.rootVisualElement);
    // }
    //
    // private void InitializeEndLevelUI(VisualElement root)
    // {
    //     endLevelMenuContainerVisualElement = root.Q<VisualElement>("endLevelMenuCardsContainer");
    //
    //     var quitButton = root.Q<Button>("quitButton");
    //     if (quitButton != null)
    //         quitButton.clicked += QuitGame;
    //
    //     UpdateUI();
    // }

    
}
