using System;
using UnityEngine;

public class ApplicationReceiver : MonoBehaviour
{
    private EventBus _eventBus;

    private ApplicationStateStorage _applicationStateStorage;

    private SimpleStateMachine<IApplicationState> _applicationStateMachine;

    private bool _isInited;

    public void Init(EventBus eventBus, ApplicationStateStorage applicationStateStorage)
    {
        //Validator.ValidateNotNull(eventBus,
            //applicationStateStorage);

        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _applicationStateStorage = applicationStateStorage ? applicationStateStorage : throw new ArgumentNullException(nameof(applicationStateStorage));
        _applicationStateMachine = new SimpleStateMachine<IApplicationState>();

        _isInited = true;
    }

    public ApplicationStateStorage ApplicationStateStorage => _applicationStateStorage;

    public IStateMachine<IApplicationState> ApplicationStateMachine => _applicationStateMachine;

    private void OnEnable()
    {
        _applicationStateMachine?.SwitchState(_applicationStateStorage.OnEnableApplicationState);
    }

    public void Start()
    {
        //if (_isInited == false)
        //{
        //    return;
        //}

        _applicationStateMachine.SwitchState(_applicationStateStorage.StartApplicationState);

        _eventBus.Invoke(new EnabledSignal<ApplicationSignal>());
    }

    private void Update()
    {
        _applicationStateStorage.UpdateApplicationState.Update();
    }

    private void OnDisable()
    {
        _applicationStateMachine.SwitchState(_applicationStateStorage.OnDisableApplicationState);

        _eventBus.Invoke(new DisabledSignal<ApplicationSignal>());
    }

    private void OnDestroy()
    {
        _applicationStateMachine.SwitchState(_applicationStateStorage.OnDestroyApplicationState);

        _eventBus.Invoke(new ClearedSignal<ApplicationSignal>());
    }

    private void OnApplicationFocus(bool focus)
    {
        _applicationStateStorage.FocusApplicationState.SetIsActive(focus);
        _applicationStateMachine.SwitchState(_applicationStateStorage.FocusApplicationState);
    }

    private void OnApplicationPause(bool pause)
    {
        _applicationStateStorage.PauseApplicationState.SetIsActive(pause);
        _applicationStateMachine.SwitchState(_applicationStateStorage.PauseApplicationState);
    }

    private void OnApplicationQuit()
    {
        _applicationStateStorage.QuitApplicationState.Trigger();
        _applicationStateMachine.SwitchState(_applicationStateStorage.QuitApplicationState);
    }
}