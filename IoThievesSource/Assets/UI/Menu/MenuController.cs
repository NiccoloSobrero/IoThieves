using UnityEngine;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    protected UIDocument menuDocument;
    
    protected void OnEnable()
    {
        var root = menuDocument.rootVisualElement;
        // var playButton = root.Q<Button>("playButton");
        var saveButton = root.Q<Button>("saveButton");
        var optionsButton = root.Q<Button>("optionsButton");
        var quitButton = root.Q<Button>("quitButton");
        // playButton.clicked += PlayGame;
        saveButton.clicked += SaveGame;
        optionsButton.clicked += OpenGameOptions;
        quitButton.clicked += QuitGame;
    }
    
    private void SaveGame()
    {
        print("save game");
    }
    
    private void OpenGameOptions()
    {
        print("open game options");
    }
    
    private void QuitGame()
    {
        print("quit game");
    }
}
