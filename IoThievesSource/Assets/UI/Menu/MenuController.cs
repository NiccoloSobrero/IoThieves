using Events;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    protected UIDocument menuDocument;
    
    protected void OnEnable()
    {
        var root = menuDocument.rootVisualElement;
        var saveButton = root.Q<Button>("saveButton");
        var optionsButton = root.Q<Button>("optionsButton");
        var quitButton = root.Q<Button>("quitButton");
        saveButton.clicked += SaveGame;
        optionsButton.clicked += OpenGameOptions;
        quitButton.clicked += QuitGame;
    }
    
    private void SaveGame()
    {
        print("save game");
        // EventsManager.OnGameSaved?.Invoke();
    }
    
    private void OpenGameOptions()
    {
        print("open game options");
    }
    
    private void QuitGame()
    {
        print("quit game");
        // temporary logic: move it in OnGameQuit Event
        EventsManager.OnGameQuit?.Invoke();
        # if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
