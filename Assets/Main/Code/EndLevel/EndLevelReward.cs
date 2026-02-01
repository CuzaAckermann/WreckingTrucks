using System;
using UnityEngine;

public class EndLevelReward : ICommandCreator
{
    private readonly ITargetPositionDefiner _targetPositionDefiner;
    private readonly float _interval;

    private Dispencer _dispencer;

    private Command _currentCommand;

    public EndLevelReward(ITargetPositionDefiner targetPositionDefiner,
                          float interval)
    {
        _targetPositionDefiner = targetPositionDefiner ?? throw new ArgumentNullException(nameof(targetPositionDefiner));
        _interval = interval > 0 ? interval : throw new ArgumentOutOfRangeException(nameof(interval));
    }

    public event Action SpaceEmpty;

    public event Action<IDestroyable> DestroyedIDestroyable;

    public event Action<Command> CommandCreated;

    public void Destroy()
    {
        _currentCommand?.Cancel();

        _currentCommand = null;

        DestroyedIDestroyable?.Invoke(this);
    }

    public void StartCollectingCartrigeBoxes(Dispencer dispencer)
    {
        _dispencer = dispencer ?? throw new ArgumentNullException(nameof(dispencer));

        SendCommand();
    }

    private void TakeCartrigeBox()
    {
        if (_dispencer.TryGetCartrigeBox(out CartrigeBox cartrigeBox) == false)
        {
            SpaceEmpty?.Invoke();

            return;
        }

        Vector3 targetPosition = _targetPositionDefiner.GetTargetPosition();
        cartrigeBox.SetTargetPosition(targetPosition);

        SendCommand();
    }

    private void SendCommand()
    {
        _currentCommand = new Command(TakeCartrigeBox, _interval);

        CommandCreated?.Invoke(_currentCommand);
    }
}