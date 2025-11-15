using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MenuController
{
    
    private void OnEnable()
    {
        base.OnEnable();
        var root = menuDocument.rootVisualElement;
        var playButton = root.Q<Button>("playButton");
        playButton.clicked += PlayGame;
    }

    private void PlayGame()
    {
        print("play game");
    }
    
}
