using UnityEngine.SceneManagement;

public class SceneReloader
{
    private readonly DeveloperInput _developerInput;

    public SceneReloader(DeveloperInput developerInput)
    {
        Validator.ValidateNotNull(developerInput);

        _developerInput = developerInput;

        SubscribeToBackgroundInput();
    }

    private void SubscribeToBackgroundInput()
    {
        _developerInput.ResetSceneButton.Pressed += OnResetSceneButtonPressed;
    }

    private void UnsubscribeFromBackgroundInput()
    {
        _developerInput.ResetSceneButton.Pressed += OnResetSceneButtonPressed;
    }

    private void OnResetSceneButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}