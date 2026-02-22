using System;
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
        if (_applicationStateStorage.TryGet(out UpdateApplicationState updateApplicationState) == false)
        {
            throw new InvalidOperationException();
        }

        updateApplicationState.Trigger();
    }
    
    private void OnApplicationFocus(bool focus)
    {
        if (_applicationStateStorage.TryGet(out FocusApplicationState focusApplicationState) == false)
        {
            throw new InvalidOperationException();
        }

        focusApplicationState.SetIsActive(focus);
    }

    private void OnApplicationPause(bool pause)
    {
        if (_applicationStateStorage.TryGet(out PauseApplicationState pauseApplicationState) == false)
        {
            throw new InvalidOperationException();
        }

        pauseApplicationState.SetIsActive(pause);
    }

    private void OnApplicationQuit()
    {
        if (_applicationStateStorage.TryGet(out QuitApplicationState quitApplicationState) == false)
        {
            throw new InvalidOperationException();
        }

        quitApplicationState.Trigger();
    }

    public void Prepare()
    {
        if (_applicationStateStorage.TryGet(out PrepareApplicationState prepareApplicationState) == false)
        {
            throw new InvalidOperationException();
        }

        prepareApplicationState.Trigger();
    }

    public void Launch()
    {
        if (_applicationStateStorage.TryGet(out StartApplicationState startApplicationState) == false)
        {
            throw new InvalidOperationException();
        }

        startApplicationState.Trigger();
    }

    public void Stop()
    {
        if (_applicationStateStorage.TryGet(out StopApplicationState stopApplicationState) == false)
        {
            throw new InvalidOperationException();
        }

        stopApplicationState.Trigger();
    }

    public void Finish()
    {
        if (_applicationStateStorage.TryGet(out FinishApplicationState finishApplicationState) == false)
        {
            throw new InvalidOperationException();
        }

        finishApplicationState.Trigger();
    }
}