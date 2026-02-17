using System;
using System.Collections.Generic;

public class InputLogger : IAbility
{
    private readonly IInput _input;

    private readonly Dictionary<IInputButton, Action> _buttonToHandler;

    public InputLogger(IInput input)
    {
        Validator.ValidateNotNull(input);

        _input = input;

        _buttonToHandler = new Dictionary<IInputButton, Action>
        {
            { _input.InteractButton, () => OnButtonPressed(_input.InteractButton) },
            { _input.PauseButton, () => OnButtonPressed(_input.PauseButton) },
            { _input.ResetSceneButton, () => OnButtonPressed(_input.ResetSceneButton) },
            { _input.SwitchUiButton, () => OnButtonPressed(_input.SwitchUiButton) },
            { _input.TimeFlowInput.TimeSpeedUpButton, () => OnButtonPressed(_input.TimeFlowInput.TimeSpeedUpButton) },
            { _input.TimeFlowInput.TimeSlowDownButton, () => OnButtonPressed(_input.TimeFlowInput.TimeSlowDownButton) }
        };

        for (int current = 0; current < _input.TimeFlowInput.TimeButtons.Count; current++)
        {
            ValueButton valueButton = _input.TimeFlowInput.TimeButtons[current];

            _buttonToHandler.Add(valueButton, () => OnButtonPressed(valueButton));
        }
    }

    public void Start()
    {
        foreach (var pair in _buttonToHandler)
        {
            pair.Key.Pressed += pair.Value;
        }
    }

    public void Finish()
    {
        foreach (var pair in _buttonToHandler)
        {
            pair.Key.Pressed -= pair.Value;
        }
    }

    private void OnButtonPressed(IInputButton inputButton)
    {
        string message = inputButton.GetType().Name;

        if (inputButton is ValueButton valueButton)
        {
            message += $" - {valueButton.Value}";
        }

        Logger.Log(message);
    }
}