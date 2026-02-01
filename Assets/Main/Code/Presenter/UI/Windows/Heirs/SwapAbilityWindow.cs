using System;
using UnityEngine;

public class SwapAbilityWindow : WindowOfState<SwapAbilityState>
{
    [SerializeField] private GameButton _returnButton;

    public event Action ReturnButtonPressed;

    public override void Init(SwapAbilityState gameState)
    {
        _returnButton.On();

        base.Init(gameState);
    }

    protected override void SubscribeToInteractables(SwapAbilityState swapAbilityState)
    {
        _returnButton.Pressed += OnReturnButtonPressed;
        swapAbilityState.AbilityStarting += OnAbilityStarted;
    }

    protected override void UnsubscribeFromInteractables(SwapAbilityState swapAbilityState)
    {
        _returnButton.Pressed -= OnReturnButtonPressed;
        swapAbilityState.AbilityStarting -= OnAbilityStarted;
    }

    private void OnReturnButtonPressed()
    {
        ReturnButtonPressed?.Invoke();
    }

    private void OnAbilityStarted()
    {
        _returnButton.Off();
    }
}