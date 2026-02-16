using UnityEngine;

public class ApplicationReceiver : MonoBehaviour
{
    private ApplicationStateStorage _applicationStateStorage;

    public void Init(ApplicationStateStorage applicationStateStorage)
    {
        Validator.ValidateNotNull(applicationStateStorage);

        _applicationStateStorage = applicationStateStorage;
    }

    public ApplicationStateStorage ApplicationStateStorage => _applicationStateStorage;

    private void Update()
    {
        _applicationStateStorage.UpdateApplicationState.Update();
    }
    
    private void OnApplicationFocus(bool focus)
    {
        _applicationStateStorage.FocusApplicationState.SetIsActive(focus);
    }

    private void OnApplicationPause(bool pause)
    {
        _applicationStateStorage.PauseApplicationState.SetIsActive(pause);
    }

    private void OnApplicationQuit()
    {
        _applicationStateStorage.QuitApplicationState.Trigger();
    }

    public void Prepare()
    {
        _applicationStateStorage.PrepareApplicationState.Trigger();
    }

    public void Launch()
    {
        _applicationStateStorage.StartApplicationState.Trigger();
    }

    public void Stop()
    {
        _applicationStateStorage.StopApplicationState.Trigger();
    }

    public void Finish()
    {
        _applicationStateStorage.FinishApplicationState.Trigger();
    }
}