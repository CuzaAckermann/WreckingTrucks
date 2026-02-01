using System;
using UnityEngine.SceneManagement;

public class SceneReloader
{
    private readonly BackgroundInput _backgroundInput;

    public SceneReloader(BackgroundInput backgroundInput)
    {
        _backgroundInput = backgroundInput ?? throw new ArgumentNullException(nameof(backgroundInput));

        SubscribeToBackgroundInput();
    }

    private void SubscribeToBackgroundInput()
    {
        _backgroundInput.ReloadScenePressed += OnResetSceneButtonPressed;

    }

    private void UnsubscribeFromBackgroundInput()
    {
        _backgroundInput.ReloadScenePressed -= OnResetSceneButtonPressed;
    }

    private void OnResetSceneButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}