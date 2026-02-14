using UnityEngine;

public class ApplicationStateStorage : MonoBehaviour
{
    private OnEnableApplicationState _onEnableApplicationState;
    private StartApplicationState _startApplicationState;

    private UpdateApplicationState _updateApplicationState;

    private OnDisableApplicationState _onDisableApplicationState;
    private OnDestroyApplicationState _onDestroyApplicationState;

    private FocusApplicationState _focusApplicationState;
    private PauseApplicationState _pauseApplicationState;
    private QuitApplicationState _quitApplicationState;

    public void Init()
    {
        _onEnableApplicationState = new OnEnableApplicationState();
        _startApplicationState = new StartApplicationState();

        _updateApplicationState = new UpdateApplicationState();

        _onDisableApplicationState = new OnDisableApplicationState();
        _onDestroyApplicationState = new OnDestroyApplicationState();

        _focusApplicationState = new FocusApplicationState();
        _pauseApplicationState = new PauseApplicationState();
        _quitApplicationState = new QuitApplicationState();
    }

    public OnEnableApplicationState OnEnableApplicationState => _onEnableApplicationState;

    public StartApplicationState StartApplicationState => _startApplicationState;

    public UpdateApplicationState UpdateApplicationState => _updateApplicationState;

    public OnDisableApplicationState OnDisableApplicationState => _onDisableApplicationState;

    public OnDestroyApplicationState OnDestroyApplicationState => _onDestroyApplicationState;

    public FocusApplicationState FocusApplicationState => _focusApplicationState;

    public PauseApplicationState PauseApplicationState => _pauseApplicationState;

    public QuitApplicationState QuitApplicationState => _quitApplicationState;
}
