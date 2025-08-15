using System;
using UnityEngine;

public class ShopWindow : WindowOfState<ShopState>
{
    [SerializeField] private GameButton _returnButton;

    public event Action ReturnButtonPressed;

    protected override void SubscribeToInteractables(ShopState gameState)
    {
        _returnButton.Pressed += OnReturnButtonPressed;
    }

    protected override void UnsubscribeFromInteractables(ShopState gameState)
    {
        _returnButton.Pressed -= OnReturnButtonPressed;
    }

    private void OnReturnButtonPressed()
    {
        ReturnButtonPressed?.Invoke();
    }
}