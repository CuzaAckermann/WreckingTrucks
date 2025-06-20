using System;
using UnityEngine;

public class BackgroundGameWindow : Window
{
    [SerializeField] private GameButton _showMainMenuButton;

    public event Action ShowMainMenuButtonPressed;

    protected override void SubscribeToInteractables()
    {
        _showMainMenuButton.Pressed += OnShowMainMenuButtonPressed;
    }

    protected override void UnsubscribeFromInteractables()
    {
        _showMainMenuButton.Pressed -= OnShowMainMenuButtonPressed;
    }

    private void OnShowMainMenuButtonPressed()
    {
        ShowMainMenuButtonPressed?.Invoke();
    }
}