using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndLevelMenuController : MonoBehaviour
{
    [SerializeField]
    protected UIDocument menuDocument;
    
    private List<string> _documents = new List<string>();

    private void OnEnable()
    {
        var root = menuDocument.rootVisualElement;
        // var resumeButton = root.Q<Button>("resumeButton");
        var container = root.Q<VisualElement>("endLevelMenuContainer");
        var firstDocument = new Button();
        firstDocument.text = "Documento 1";
        container.Add(firstDocument);
        
    }

    public void SetupDocuments(List<string> docs)
    {
        print("SetupDocuments");
        print(docs.Count);
        _documents = docs;
    }
}
