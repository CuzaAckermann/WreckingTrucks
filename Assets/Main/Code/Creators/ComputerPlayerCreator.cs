using System;

public class ComputerPlayerCreator
{
    private readonly EventBus _eventBus;
    private readonly TypesCalculatorCreator _typesCalculatorCreator;
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly ComputerPlayerSettings _computerPlayerSettings;

    public ComputerPlayerCreator(ComputerPlayerSettings computerPlayerSettings,
                                 StopwatchCreator stopwatchCreator,
                                 TypesCalculatorCreator typesCalculatorCreator,
                                 EventBus eventBus)
    {
        _computerPlayerSettings = computerPlayerSettings ?? throw new ArgumentNullException(nameof(computerPlayerSettings));
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _typesCalculatorCreator = typesCalculatorCreator ?? throw new ArgumentNullException(nameof(typesCalculatorCreator));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public ComputerPlayer Create()
    {
        return new ComputerPlayer(_eventBus,
                                  new TruckSelector(_eventBus,
                                                    _typesCalculatorCreator.Create()),
                                  _stopwatchCreator.Create(),
                                  _computerPlayerSettings.StartDelay,
                                  _computerPlayerSettings.MinFrequency,
                                  _computerPlayerSettings.MaxFrequency);
    }
}