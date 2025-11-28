using UnityEngine;
using UnityEngine.UIElements;
using Events;

public class GameOverMenuController : MenuController
{
    private void OnEnable()
    {
        base.OnEnable();
        var root = menuDocument.rootVisualElement;
        var restartButton = root.Q<Button>("restartButton");
        restartButton.clicked += RestartGame;
    }

    private void RestartGame()
    {
        EventsManager.OnRestartGame?.Invoke();
    }
}
