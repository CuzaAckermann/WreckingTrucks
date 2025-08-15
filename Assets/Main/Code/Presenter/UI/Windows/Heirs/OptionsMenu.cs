using System;
using UnityEngine;

public class OptionsMenu : WindowOfState<OptionsMenuState>
{
    [SerializeField] private GameButton _returnButton;

    public event Action ReturnButtonPressed;

    protected override void SubscribeToInteractables(OptionsMenuState optionsMenuState)
    {
        _returnButton.Pressed += OnReturnButtonPressed;
    }

    protected override void UnsubscribeFromInteractables(OptionsMenuState optionsMenuState)
    {
        _returnButton.Pressed -= OnReturnButtonPressed;
    }

    private void OnReturnButtonPressed()
    {
        ReturnButtonPressed?.Invoke();
    }
}