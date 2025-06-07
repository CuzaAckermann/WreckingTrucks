using UnityEngine;

public class KeyboardPlayingInputHandlerCreator : MonoBehaviour
{
    [SerializeField] private KeyCode _pauseButton = KeyCode.Escape;
    [SerializeField] private KeyCode _interactButton = KeyCode.Mouse0;
    [SerializeField] private bool _isOneClick = true;

    public KeyboardPlayingInputHandler CreateKeyboardPlayingInputHandler()
    {
        return new KeyboardPlayingInputHandler(new KeyboardPauseInput(_pauseButton),
                                               new KeyboardInteractInput(_interactButton,
                                                                         _isOneClick));
    }
}