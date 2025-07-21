using UnityEngine;

[CreateAssetMenu(fileName = "NewKeyboardInputSettings", menuName = "Settings/New Keyboard Input Settings")]
public class KeyboardInputSettings : ScriptableObject
{
    [SerializeField] private KeyCode _pauseButton = KeyCode.Escape;
    [SerializeField] private KeyCode _interactButton = KeyCode.Mouse0;
    [SerializeField] private bool _isOneClick = true;

    public KeyCode PauseButton => _pauseButton;

    public KeyCode InteractButton => _interactButton;

    public bool IsOneClick => _isOneClick;
}