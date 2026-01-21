using System;
using UnityEngine;

public class EndLevelReward
{
    private readonly ITargetPositionDefiner _targetPositionDefiner;
    private readonly Stopwatch _stopwatch;

    private Dispencer _dispencer;

    public EndLevelReward(ITargetPositionDefiner targetPositionDefiner,
                          Stopwatch stopwatch)
    {
        _targetPositionDefiner = targetPositionDefiner ?? throw new ArgumentNullException(nameof(targetPositionDefiner));
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
    }

    public event Action SpaceEmpty;

    public void StartCollectingCartrigeBoxes(Dispencer dispencer)
    {
        _dispencer = dispencer ?? throw new ArgumentNullException(nameof(dispencer));

        _stopwatch.IntervalPassed += TakeCartrigeBox;
        _stopwatch.SetNotificationInterval(0.005f);
        _stopwatch.Start();
    }

    private void TakeCartrigeBox()
    {
        if (_dispencer.TryGetCartrigeBox(out CartrigeBox cartrigeBox))
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