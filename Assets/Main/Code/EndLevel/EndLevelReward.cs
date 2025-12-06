using System;
using System.Collections.Generic;
using UnityEngine;

// идея хорошая, но нужно подправить
public class EndLevelReward
{
    private readonly ITargetPositionDefiner _targetPositionDefiner;
    private readonly Stopwatch _stopwatch;

    private CartrigeBoxField _cartrigeBoxField;

    public EndLevelReward(ITargetPositionDefiner targetPositionDefiner,
                          Stopwatch stopwatch)
    {
        _targetPositionDefiner = targetPositionDefiner ?? throw new ArgumentNullException(nameof(targetPositionDefiner));
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
    }

    public event Action SpaceEmpty;

    public void StartCollectingCartrigeBoxes(CartrigeBoxField cartrigeBoxField)
    {
        _cartrigeBoxField = cartrigeBoxField ?? throw new ArgumentNullException(nameof(cartrigeBoxField));

        _stopwatch.IntervalPassed += TakeCartrigeBox;
        _stopwatch.SetNotificationInterval(0.005f);
        _stopwatch.Start();
    }

    private void TakeCartrigeBox()
    {
        if (_cartrigeBoxField.TryGetCartrigeBox(out CartrigeBox cartrigeBox))
        {
            cartrigeBox.DestroyedModel += OnDestroyed;
            cartrigeBox.TargetPositionReached += OnTargetPositionReached;

            Vector3 targetPosition = _targetPositionDefiner.GetTargetPosition();
            cartrigeBox.SetTargetPosition(targetPosition);
        }
        else
        {
            _stopwatch.IntervalPassed -= TakeCartrigeBox;
            _stopwatch.Stop();
            SpaceEmpty?.Invoke();
        }
    }

    private void OnTargetPositionReached(Model model)
    {
        OnDestroyed(model);
    }

    private void OnDestroyed(Model model)
    {
        model.DestroyedModel -= OnDestroyed;
        model.TargetPositionReached -= OnTargetPositionReached;
    }
}