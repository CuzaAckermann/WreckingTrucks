using UnityEngine;

[CreateAssetMenu(fileName = "NewKeyboardInputSettings", menuName = "Settings/Keyboard Input Settings")]
public class KeyboardInputSettings : ScriptableObject
{
    [Header("Game Keys")]
    [SerializeField] private KeyCode _pauseButton = KeyCode.Escape;
    [SerializeField] private KeyCode _interactButton = KeyCode.Mouse0;
    [SerializeField] private bool _isOneClick = true;

    [Header("Time Management Keys")]
    [SerializeField] private TimeButton _verySlowTimeButton;
    [SerializeField] private TimeButton _slowTimeButton;
    [SerializeField] private TimeButton _normalTimeButton;
    [SerializeField] private TimeButton _fastTimeButton;
    [SerializeField] private TimeButton _veryFastTimeButton;

    [Header("Increase and Decrease")]
    [SerializeField] private TimeButton _decreasedTimeButton;
    [SerializeField] private TimeButton _increasedTimeButton;

    [Header("Service Keys")]
    [SerializeField] private KeyCode _resetSceneButton = KeyCode.R;

    public KeyCode PauseButton => _pauseButton;

    public KeyCode InteractButton => _interactButton;

    public bool IsOneClick => _isOneClick;

    public TimeButton VerySlowTimeButton => _verySlowTimeButton;

    public TimeButton SlowTimeButton => _slowTimeButton;

    public TimeButton NormalTimeButton => _normalTimeButton;

    public TimeButton FastTimeButton => _fastTimeButton;

    public TimeButton VeryFastTimeButton => _veryFastTimeButton;

    public TimeButton DecreasedTimeButton => _decreasedTimeButton;

    public TimeButton IncreasedTimeButton => _increasedTimeButton;

    public KeyCode ResetSceneButton => _resetSceneButton;
}