using System;
using System.Collections.Generic;
using UnityEngine;

// идея хорошая, но нужно подправить
public class EndLevelReward : IModelDestroyNotifier, IModelPositionObserver
{
    private readonly ITargetPositionDeterminator _targetPositionDeterminator;
    private readonly Stopwatch _stopwatch;

    private CartrigeBoxSpace _cartrigeBoxSpace;

    public EndLevelReward(ITargetPositionDeterminator targetPositionDeterminator,
                          Stopwatch stopwatch)
    {
        _targetPositionDeterminator = targetPositionDeterminator ?? throw new ArgumentNullException(nameof(targetPositionDeterminator));
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
    }

    public event Action SpaceEmpty;

    public event Action<Model> ModelDestroyRequested;
    public event Action<IReadOnlyList<Model>> ModelsDestroyRequested;

    public event Action<Model> PositionChanged;
    public event Action<List<Model>> PositionsChanged;

    public void TakeCartrigeBoxes(CartrigeBoxSpace cartrigeBoxSpace)
    {
        _cartrigeBoxSpace = cartrigeBoxSpace ?? throw new ArgumentNullException(nameof(cartrigeBoxSpace));

        _stopwatch.IntervalPassed += TakeCartrigeBox;
        _stopwatch.SetNotificationInterval(0.01f);
        _stopwatch.Start();
    }

    private void TakeCartrigeBox()
    {
        if (_cartrigeBoxSpace.TryGetCartrigeBox(out CartrigeBox cartrigeBox))
        {
            cartrigeBox.Destroyed += OnDestroyed;
            cartrigeBox.TargetPositionReached += OnTargetPositionReached;

            Vector3 targetPosition = _targetPositionDeterminator.GetTargetPosition();
            cartrigeBox.SetTargetPosition(targetPosition);
            PositionChanged?.Invoke(cartrigeBox);
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

        ModelDestroyRequested?.Invoke(model);
    }

    private void OnDestroyed(Model model)
    {
        model.Destroyed -= OnDestroyed;
        model.TargetPositionReached -= OnTargetPositionReached;
    }
}