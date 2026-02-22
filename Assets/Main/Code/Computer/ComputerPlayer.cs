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
        Validator.ValidateNotNull(eventBus, truckSelector);
        Validator.ValidateMax(minFrequency, maxFrequency, true);
        Validator.ValidateMin(startDelay, 0, true);
        Validator.ValidateMin(minFrequency, 0, true);
        Validator.ValidateMin(maxFrequency, 0, true);

        _eventBus = eventBus;
        _truckSelector = truckSelector;

        _startDelay = startDelay;
        _minFrequency = minFrequency;
        _maxFrequency = maxFrequency;
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