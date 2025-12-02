using System;
using System.Collections.Generic;
using UnityEngine;

// идея хорошая, но нужно подправить
public class EndLevelReward
{
    private readonly ITargetPositionDeterminator _targetPositionDeterminator;
    private readonly Stopwatch _stopwatch;

    private CartrigeBoxField _cartrigeBoxField;

    public EndLevelReward(ITargetPositionDeterminator targetPositionDeterminator,
                          Stopwatch stopwatch)
    {
        _targetPositionDeterminator = targetPositionDeterminator ?? throw new ArgumentNullException(nameof(targetPositionDeterminator));
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
    }

    public event Action SpaceEmpty;

    public void TakeCartrigeBoxes(CartrigeBoxField cartrigeBoxField)
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
            cartrigeBox.Destroyed += OnDestroyed;
            cartrigeBox.TargetPositionReached += OnTargetPositionReached;

            Vector3 targetPosition = _targetPositionDeterminator.GetTargetPosition();
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
        model.Destroyed -= OnDestroyed;
        model.TargetPositionReached -= OnTargetPositionReached;
    }
}