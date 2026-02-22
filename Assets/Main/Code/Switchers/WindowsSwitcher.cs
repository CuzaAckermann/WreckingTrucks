using System.Collections.Generic;

public class WindowsSwitcher : IApplicationAbility
{
    private readonly StateWindowStorage _stateWindowStorage;

    private readonly List<StateWindowBase> _stateWindows;
    private readonly InputStateMachine _inputStateMachine;

    private StateWindowBase _currentWindow;

    public WindowsSwitcher(StateWindowStorage stateWindowStorage,
                           InputStateMachine inputStateMachine,
                           int amountLevels)
    {
        Validator.ValidateNotNull(stateWindowStorage, inputStateMachine);
        Validator.ValidateMin(amountLevels, 0, true);

        _stateWindowStorage = stateWindowStorage;
        _stateWindows = _stateWindowStorage.GetAll();
        _inputStateMachine = inputStateMachine;

        if (_stateWindowStorage.TryGet(out SwapAbilityWindow swapAbilityWindow))
        {
            swapAbilityWindow.Init();
        }

        if (_stateWindowStorage.TryGet(out LevelButtonsStorage levelButtonsStorage))
        {
            levelButtonsStorage.Init(amountLevels);
        }

        HideAllWindows();
    }

    public void Start()
    {
        _inputStateMachine.StateChanged += SwitchWindow;

        if (_inputStateMachine.CurrentState == null)
        {
            return;
        }

        SwitchWindow(_inputStateMachine.CurrentState);
    }

    public void Finish()
    {
        _inputStateMachine.StateChanged -= SwitchWindow;
    }

    private void HideAllWindows()
    {
        foreach (StateWindowBase stateWindowBase in _stateWindows)
        {
            stateWindowBase.Hide();
        }
    }

    private void SwitchWindow(InputState inputState)
    {
        Validator.ValidateNotNull(inputState);

        if (_currentWindow != null)
        {
            _currentWindow.Hide();
        }

        FindNextWindow(inputState);

        _currentWindow.Show();
    }

    private void FindNextWindow(InputState inputState)
    {
        foreach (StateWindowBase stateWindowBase in _stateWindows)
        {
            if (inputState.GetType() == stateWindowBase.BoundStateType)
            {
                _currentWindow = stateWindowBase;

                return;
            }
        }
    }
}