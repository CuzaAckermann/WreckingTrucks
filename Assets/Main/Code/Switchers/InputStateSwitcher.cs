using System;
using System.Collections.Generic;

public class InputStateSwitcher : IApplicationAbility
{
    private readonly UpdateApplicationState _updateApplicationState;
    private readonly LevelCreator _levelCreator;
    private readonly EventBus _eventBus;

    private readonly DeveloperInputState _developerInputState;
    private readonly InputStateMachine _inputStateMachine;

    private readonly InputStateStorage _inputStateStorage;

    private readonly List<WindowHandlerBase> _windowHandlers;

    public InputStateSwitcher(UpdateApplicationState updateApplicationState,
                              LevelCreator levelCreator,
                              EventBus eventBus,
                              InputStateStorage inputStateStorage,
                              InputStateMachine inputStateMachine,
                              List<WindowHandlerBase> windowHandlers)
    {
        Validator.ValidateNotNull(updateApplicationState, levelCreator, eventBus, inputStateStorage, inputStateMachine, windowHandlers);

        _updateApplicationState = updateApplicationState;
        _levelCreator = levelCreator;
        _eventBus = eventBus;
        _inputStateStorage = inputStateStorage;

        _inputStateMachine = inputStateMachine;

        if (_inputStateStorage.TryGet(out DeveloperInputState developerInputState) == false)
        {
            throw new InvalidOperationException();
        }

        _developerInputState = developerInputState;

        _windowHandlers = windowHandlers;
    }

    public InputStateMachine InputStateMachine => _inputStateMachine;

    public void Start()
    {
        //_levelCreator.Created += ;

        _eventBus.Subscribe<CreatedSignal<Level>>(FinishPlayingState, Priority.High);
        _eventBus.Subscribe<CreatedSignal<Level>>(PreparePlayingState, Priority.Low);

        _updateApplicationState.Triggered += Update;

        SubscribeToHandlers();

        ResetStates();
    }

    public void Finish()
    {
        //_levelCreator.Created -= ;

        _eventBus.Unsubscribe<CreatedSignal<Level>>(FinishPlayingState);
        _eventBus.Unsubscribe<CreatedSignal<Level>>(PreparePlayingState);

        _updateApplicationState.Triggered -= Update;

        UnsubscribeFromHandlers();
    }

    private void SubscribeToHandlers()
    {
        foreach (WindowHandlerBase windowHandler in _windowHandlers)
        {
            windowHandler.InputStateStarting += SetInputState;
            windowHandler.InputStateReturning += ReturnPreviousState;
        }

        foreach (WindowHandlerBase window in _windowHandlers)
        {
            window.Start();
        }
    }

    private void UnsubscribeFromHandlers()
    {
        foreach (WindowHandlerBase windowHandler in _windowHandlers)
        {
            windowHandler.Finish();
        }

        foreach (WindowHandlerBase window in _windowHandlers)
        {
            window.InputStateStarting -= SetInputState;
            window.InputStateReturning -= ReturnPreviousState;
        }
    }

    private void SetInputState(InputState inputState)
    {
        if (inputState is MainMenuInputState)
        {
            ResetStates();

            return;
        }

        _inputStateMachine.PushState(inputState);
    }

    private void ReturnPreviousState()
    {
        _inputStateMachine.PopState();
    }

    private void ResetStates()
    {
        _inputStateMachine.ClearStates();

        if (_inputStateStorage.TryGet(out MainMenuInputState mainMenuInputState) == false)
        {
            throw new InvalidOperationException();
        }

        _inputStateMachine.PushState(mainMenuInputState);
    }

    private void Update()
    {
        _developerInputState.Update();

        _inputStateMachine.Update();
    }

    // Методы ниже - это реакция на поведение LEVEL, похоже они не должны быть тут
    private void PreparePlayingState(CreatedSignal<Level> _)
    {
        _eventBus.Subscribe<CompletedSignal<Level>>(OnLevelPassed);
        _eventBus.Subscribe<FailedSignal<Level>>(OnLevelFailed);

        if (_inputStateStorage.TryGet(out PlayingInputState playingInputState) == false)
        {
            throw new InvalidOperationException();
        }

        _inputStateMachine.PushState(playingInputState);
    }

    private void FinishPlayingState(CreatedSignal<Level> _)
    {
        _eventBus.Unsubscribe<CompletedSignal<Level>>(OnLevelPassed);
        _eventBus.Unsubscribe<FailedSignal<Level>>(OnLevelFailed);
    }

    private void OnLevelPassed(CompletedSignal<Level> _)
    {
        if (_inputStateStorage.TryGet(out EndLevelInputState endLevelInputState) == false)
        {
            throw new InvalidOperationException();
        }

        _inputStateMachine.PushState(endLevelInputState);
    }

    private void OnLevelFailed(FailedSignal<Level> _)
    {
        if (_inputStateStorage.TryGet(out EndLevelInputState endLevelInputState) == false)
        {
            throw new InvalidOperationException();
        }

        _inputStateMachine.PushState(endLevelInputState);
    }
}