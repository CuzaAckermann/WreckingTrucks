using System;
using Random = UnityEngine.Random;

public class ComputerPlayer : ICommandCreator
{
    private readonly EventBus _eventBus;
    private readonly TruckSelector _truckSelector;

    private readonly float _startDelay;
    private readonly float _minFrequency;
    private readonly float _maxFrequency;

    private Command _currentCommand;

    public ComputerPlayer(EventBus eventBus,
                          TruckSelector truckSelector,
                          float startDelay,
                          float minFrequency,
                          float maxFrequency)
    {
        if (minFrequency >= maxFrequency)
        {
            throw new ArgumentOutOfRangeException(nameof(minFrequency));
        }

        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _truckSelector = truckSelector ?? throw new ArgumentNullException(nameof(truckSelector));

        _startDelay = startDelay > 0 ? startDelay : throw new ArgumentOutOfRangeException(nameof(startDelay));
        _minFrequency = minFrequency > 0 ? minFrequency : throw new ArgumentOutOfRangeException(nameof(minFrequency));
        _maxFrequency = maxFrequency > 0 ? maxFrequency : throw new ArgumentOutOfRangeException(nameof(maxFrequency));
    }

    public event Action<IDestroyable> Destroyed;

    public event Action<Command> CommandCreated;

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    public void Enable()
    {
        SendCommand(_startDelay);
    }

    public void Disable()
    {
        CancelCommand();
    }

    private void ReleaseTruck()
    {
        if (_truckSelector.TrySelectTruck(out Truck truck) == false)
        {
            SendCommand(_startDelay);

            return;
        }

        _eventBus.Invoke(new SelectedSignal<Model>(truck));

        SendCommand(Random.Range(_minFrequency, _maxFrequency));
    }

    private void SendCommand(float delay)
    {
        _currentCommand = new Command(ReleaseTruck, delay);

        CommandCreated?.Invoke(_currentCommand);
    }

    private void CancelCommand()
    {
        _currentCommand?.Cancel();

        _currentCommand = null;
    }
}