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
    // private List<string> _documents = new List<string>();

    private void OnEnable()
    {
        var root = menuDocument.rootVisualElement;
        endLevelMenuContainerVisualElement = root.Q<VisualElement>("endLevelMenuContainer");
        // // var resumeButton = root.Q<Button>("resumeButton");
        // var container = root.Q<VisualElement>("endLevelMenuContainer");
        // var firstDocument = new Button();
        // firstDocument.text = "Documento 1";
        // container.Add(firstDocument);
        UpdateUI();
    }

    private void UpdateUI()
    {
        print("Update ui");
        if (documentButtonTemplate == null)
            return;
        print("reading document");
        foreach (string document in documents)
        {
            print("in foreach");
            var documentButtonInstance = documentButtonTemplate.Instantiate();
            var nameLabel =  documentButtonInstance.Q<Label>("endLevelDescription");
            nameLabel.text = document;
            endLevelMenuContainerVisualElement.Add(documentButtonInstance);
        }
    }

    // public void SetupDocuments(List<string> docs)
    // {
    //     print("SetupDocuments");
    //     print(docs.Count);
    //     _documents = docs;
    // }
}
