using System;
using System.Collections.Generic;
using UnityEngine;

public class SwapAbility : MonoBehaviour
{
    private readonly SwapAbilityInputState _swapAbilityState;
    private readonly IInput _input;

    private readonly SphereCastPresenterDetector _blockPresenterDetector;
    private readonly BlockFieldManipulator _blockFieldManipulator;


    public SwapAbility(SwapAbilityInputState swapAbilityInputState,
                       IInput input,
                       SphereCastPresenterDetector sphereCastPresenterDetector,
                       BlockFieldManipulator blockFieldManipulator)
    {
        Validator.ValidateNotNull(swapAbilityInputState, input);
        _swapAbilityState = swapAbilityInputState;
        _input = input;

        _blockPresenterDetector = sphereCastPresenterDetector ?? throw new ArgumentNullException(nameof(sphereCastPresenterDetector));
        _blockFieldManipulator = blockFieldManipulator ?? throw new ArgumentNullException(nameof(blockFieldManipulator));
    }

    public event Action AbilityStarting;
    public event Action AbilityFinished;

    private void OnEnter()
    {
        _blockFieldManipulator.OpenField();
        _input.InteractButton.Pressed += OnInteractPressed;
    }

    private void OnExit()
    {
        _blockFieldManipulator.CloseField();
    }

    private void OnAbilityStarting()
    {
        AbilityStarting?.Invoke();
    }

    private void OnAbilityFinished()
    {
        AbilityFinished?.Invoke();
    }

    private void OnInteractPressed()
    {
        if (_blockPresenterDetector.TryGetPresenter(out Presenter blockPresenter))
        {
            if (blockPresenter.Model is Block block)
            {
                _input.InteractButton.Pressed -= OnInteractPressed;

                AbilityStarting?.Invoke();

                // тут начинаем замену одного типа блоков на другой
            }
        }
    }
}