using UnityEngine;

public class KeyboardInputHandlerCreator : MonoBehaviour
{
    [SerializeField] private KeyCode _pauseButton = KeyCode.Escape;
    [SerializeField] private KeyCode _interactButton = KeyCode.Mouse0;
    [SerializeField] private bool _isOneClick = true;

    public KeyboardPlayingInputHandler CreatePlayingInputHandler()
    {
        return new KeyboardPlayingInputHandler(new KeyboardPauseInput(_pauseButton),
                                               new KeyboardInteractInput(_interactButton,
                                                                         _isOneClick));
    }

    public KeyboardPauseInputHandler CreatePauseInputHandler()
    {
        return new KeyboardPauseInputHandler(new KeyboardPauseInput(_pauseButton));
    }
}