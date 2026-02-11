using UnityEngine;

public class ApplicationStateStorage : MonoBehaviour
{
    private FocusApplicationState _focusApplicationState;
    private PauseApplicationState _pauseApplicationState;
    private QuitApplicationState _quitApplicationState;

    public void Init()
    {
        _focusApplicationState = new FocusApplicationState();
        _pauseApplicationState = new PauseApplicationState();
        _quitApplicationState = new QuitApplicationState();
    }

    public ISwitchedApplicationState FocusApplicationState => _focusApplicationState;

    public ISwitchedApplicationState PauseApplicationState => _pauseApplicationState;

    public IApplicationState QuitApplicationState => _quitApplicationState;

    private void OnApplicationFocus(bool focus)
    {
        Logger.Log($"{nameof(OnApplicationFocus)} - {focus}");

        _focusApplicationState.SetIsActive(focus);
    }

    private void OnApplicationPause(bool pause)
    {
        Logger.Log($"{nameof(OnApplicationPause)} - {pause}");

        _pauseApplicationState.SetIsActive(pause);
    }

    private void OnApplicationQuit()
    {
        Logger.Log($"{nameof(OnApplicationQuit)}");

        _quitApplicationState.Trigger();
    }
}
