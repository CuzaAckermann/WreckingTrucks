using System;
using UnityEngine;

public class OptionsMenu : Window
{
    [SerializeField] private GameButton _returnButton;

    public event Action ReturnButtonPressed;

    protected override void SubscribeToInteractables()
    {
        _returnButton.Pressed += OnReturnButtonPressed;
    }

    protected override void UnsubscribeFromInteractables()
    {
        _returnButton.Pressed -= OnReturnButtonPressed;
    }

    private void OnReturnButtonPressed()
    {
        ReturnButtonPressed?.Invoke();
    }
}