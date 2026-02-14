using UnityEngine;

[CreateAssetMenu(fileName = "NewKeyboardInputSettings", menuName = "Settings/Keyboard Input Settings")]
public class KeyboardInputSettings : ScriptableObject
{
    [Header("Game Keys")]
    [SerializeField] private KeyCode _pauseButton = KeyCode.Escape;
    [SerializeField] private KeyCode _interactButton = KeyCode.Mouse0;
    [SerializeField] private PressModeEnum _pressMode = PressModeEnum.Down;

    [Header("Time Flow Settings")]
    [SerializeField] private TimeFlowSettings _timeFlowSettings;

    [Header("Service Keys")]
    [SerializeField] private KeyCode _resetSceneButton = KeyCode.R;
    [SerializeField] private KeyCode _switchUiButton = KeyCode.U;

    public KeyCode PauseButton => _pauseButton;

    public KeyCode InteractButton => _interactButton;

    public PressModeEnum PressMode => _pressMode;

    public TimeFlowSettings TimeFlowSettings => _timeFlowSettings;

    public KeyCode ResetSceneButton => _resetSceneButton;

    public KeyCode SwitchUiButton => _switchUiButton;
}