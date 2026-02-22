using UnityEngine.SceneManagement;

public class SceneReloader : IApplicationAbility
{
    private readonly IInput _input;

    public SceneReloader(IInput input)
    {
        Validator.ValidateNotNull(input);

        _input = input;
    }

    public void Start()
    {
        _input.ResetSceneButton.Pressed += OnResetSceneButtonPressed;
    }

    public void Finish()
    {
        _input.ResetSceneButton.Pressed -= OnResetSceneButtonPressed;
    }

    private void OnResetSceneButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}