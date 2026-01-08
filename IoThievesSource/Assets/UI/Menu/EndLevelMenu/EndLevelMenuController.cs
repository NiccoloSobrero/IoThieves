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
    // private List<FileDetails> documents = new List<FileDetails>();
    
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

    private void UpdateUI()
    {
        if (documentCardTemplate == null)
            return;
        menuDocument.rootVisualElement.style.display = DisplayStyle.None;
        // foreach (FileDetails document in documents)
        // {
        //     var documentButtonInstance = documentCardTemplate.Instantiate();
        //     var nameLabel =  documentButtonInstance.Q<Label>("cardTitle");
        //     nameLabel.text = document.filePath;
        //     var image = documentButtonInstance.Q<Image>("cardImage");
        //     image.sprite = document.documentSprite;
        //     endLevelMenuContainerVisualElement.Add(documentButtonInstance);
        // }
    }
    
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
    
}
